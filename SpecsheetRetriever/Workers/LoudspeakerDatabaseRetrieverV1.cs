using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using SpecsheetRetriever.Interfaces;
using SpecsheetRetriever.Models;
using SpecsheetRetriever.Utils;

namespace SpecsheetRetriever.Workers
{
    internal class LoudspeakerDatabaseRetrieverV1 : IRetriever
    {
        private const string SourceName = "Loudspeaker Database";
        private const string BaseUrl = "http://www.loudspeakerdatabase.com/";
        private readonly Uri BaseUri = new Uri(BaseUrl);

        #region Get Specsheets

        private static string TranslateBrand(SearchItem searchItem)
        {
            return searchItem.Brand switch
            {
                // TODO: This needs to be manually checked.
                "Eighteen Sound" => "18Sound",
                "FaitalPRO" => "Faital",

                _ => searchItem.Brand.Replace(" ", null)
            };

            /*
"accuton"	accuton
"AE"	    Acoustic Elegance
"ATOHM"	    ATOHM
"Audes"	    Audes
"AurumCantus"	Aurum Cantus
"Avatar"	Avatar Audio
"BC"	    B&C Speaker
"Beyma"	    Beyma
"BlackHydra"	Black Hydra
"BMS"	    BMS
"BOSS"	    BOSS Audio
"BRAX"	    BRAX
"Celestion"	Celestion
"CerwinVega"	Cerwin Vega
"Ciare"	    Ciare
"CT"	    CT Sounds
"DAS"	    D.A.S. Audio
"Dayton"	Dayton Audio
"DB"	    DB Drive
"DD"	    DD Audio
"DeafBonce"	Deaf Bonce
"18Sound"	Eighteen Sound
"Eminence"	Eminence
"ESX"	    ESX
"ETON"	    ETON
"Faital"	FaitalPRO
"FANE"	    Fane
"Fi"	    Fi Car Audio
"Fostex"	Fostex
"Fountek"	Fountek
"Goldwood"	Goldwood
"GRS"	    GRS
"HELIX"	    HELIX
"Hertz"	    Hertz
"Jensen"	Jensen
"JL"	    JL Audio
"KICX"	    KICX
"LaVoce"	LaVoce
"Lowther"	Lowther
"MATCH"	    MATCH
"MDLab"	    MD Lab
"mivoc"	    mivoc
"MONACOR"	MONACOR
"Morel"	    Morel
"MTX"	    MTX Audio
"Oberton"	Oberton
"Peerless"	Peerless by Tymphany
"Pioneer"	Pioneer
"PlanetAudio"	Planet Audio
"PowerBass"	PowerBass
"PrecisionDevices"	Precision Devices
"Pride"	    Pride
"PRV"	    PRV Audio
"Radian"	Radian
"RCF"	    RCF
"RockfordFosgate"	Rockford Fosgate
"SB"	    SB Acoustics
"ScanSpeak"	Scan-Speak
"SEAS"	    SEAS
"SICA"	    SICA
"Skar"	    Skar Audio
"SSL"	    Sound Storm Lab
"SUPRAVOX"	SUPRAVOX
"TangBand"	Tang Band
"VISATON"	VISATON
"VOLT"	    VOLT
"Wavecor"	Wavecor
"WVL"	    WVL
             */
        }

        private static string TranslateModel(SearchItem searchItem)
        {
            if (searchItem.OhmVersion.HasValue) //Ω
            {
                return searchItem.Model + $"_{searchItem.OhmVersion}Ω";
            }

            return searchItem.Model;
        }

        public async Task<SpecsheetItem?> SearchItem(SearchItem searchItem, CancellationToken cToken)
        {
            if (cToken.IsCancellationRequested) { return null; }

            string url;
            string? sheetString;
            string translatedBrand = TranslateBrand(searchItem);
            string translatedModel = TranslateModel(searchItem);

            // First try a direct guess.
            url = $"{BaseUrl}VituixCAD/{translatedBrand}_{translatedModel}.xml";
            sheetString = await SharedHttpClient.GetContentString(url, cToken);
            if (cToken.IsCancellationRequested) { return null; }

            // If failed, try to find the link on the page.
            if (string.IsNullOrEmpty(sheetString))
            {
                url = $"{BaseUrl}{translatedBrand}/{translatedModel}";
                string? guessedUrl = await SearchLink(url, cToken);
                if (cToken.IsCancellationRequested) { return null; }

                if (!string.IsNullOrEmpty(guessedUrl))
                {
                    sheetString = await SharedHttpClient.GetContentString(guessedUrl, cToken);
                }
            }

            if (string.IsNullOrEmpty(sheetString))
            {
                return null;
            }

            return new SpecsheetItem()
            {
                DataSource = SourceName,
                Brand = searchItem.Brand,
                Model = searchItem.Model,
                Payload = sheetString
            };
        }

        private async Task<string?> SearchLink(string url, CancellationToken cToken)
        {
            // Read driver page source.
            string? str = await SharedHttpClient.GetContentString(url, cToken);

            if (string.IsNullOrEmpty(str))
            {
                return null;
            }

            // Try to find the link to VCad file.
            var re = GetLinkSearchRegex();
            var match = re.Match(str);

            if (match.Success)
            {
                // href="/VituixCAD/18Sound_10NMB420.xml"
                const int StartLen = 6;
                string? found = match.Captures.FirstOrDefault()?.Value;

                if (found == null)
                {
                    return null;
                }

                found = found.Substring(StartLen, found.Length - StartLen - 1);
                Uri uri = new Uri(BaseUri, found);

                return uri.ToString();
            }

            return null;
        }

        Regex? _linkSearchRegex; // Cache.

        private Regex GetLinkSearchRegex()
        {
            if (_linkSearchRegex != null)
            {
                return _linkSearchRegex;
            }

            // Regex for finding link href
            _linkSearchRegex = new Regex(@"href=""(http(s)?:\/\/(www.)?loudspeakerdatabase.com)?(\/)?VituixCAD\/([\w\d_\.\/])+\.xml""", RegexOptions.IgnoreCase);

            return _linkSearchRegex;
        }

        #endregion

    }
}
