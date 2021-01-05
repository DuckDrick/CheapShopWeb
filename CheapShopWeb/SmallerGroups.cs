using System;
using System.Collections.Generic;

namespace Comparison_shopping_engine
{
    internal class SmallerGroups
    {
        public bool Check(string productgroup, List<string> smallergroup)
        {
            foreach (var group in smallergroup)
                if (productgroup.Contains(group))
                    return true;

            return false;
        }

        public List<string> KidsGroup()//
        {
            string[] group =
            {
                "Žaislai, prekės vaikams", "Gimtadienio atributika", "Kūdikių higienos prekės", "Vaikams ir kūdikiams",
                "DOVANOS, ŠVENTINĖ ATRIBUTIKA", "VAIKUI IR MAMAI, ŽAISLAI"
            };
            return new List<string>(group);
        }

        public List<string> ClothingGroup()//
        {
            string[] group = {"Apranga, avalynė, aksesuarai"};
            return new List<string>(group);
        }

        public List<string> CarGroup()//
        {
            string[] group =
            {
                "Ratų varžtai", "Kėbulo apsaugos, deflektoriai", "Stabilizatoriaus traukės, stabilizatoriai",
                "Diržai juostiniai", "Pedalų gumos, pedalų laikikliai", "Variklio dangtelio elementai",
                "Glaistymo ir gruntavimo medžiagos", "Uždegimo žvakės",
                "Juostinių diržų įtempikliai, įtempimo skriemuliai", "Variklio galvos tarpinės",
                "Vairaračio užvalkalai ir priedai", "Išorės priežiūros priemonės", "Priemonės nuo uodų ir erkių",
                "Purvasaugiai, įkrovos nuėmėjai", "Langų,žibintų plovimo varikliukai", "Veidrodžiai",
                "Variklio įsiurbimo/išmetimo kolektorių tarpikliai", "Variklio skriemuliai, krumpliaračiai",
                "Stiklų valikliai ir poliroliai", "Kempinės ir servetėlės",
                "Variklio alyvos siurblys, siurblio priedai", "Paskirstymo diržų montavimo komplektai",
                "Pusašiai, lankstai", "Pavarų svirties rankenos", "Bagažinės pertvaros ir grotelės",
                "Laikikliai ir fiksatoriai", "Motociklų ir motorolerių dalys", "Stabilizatoriaus įvorės",
                "Įvairūs riebokšliai", "Ratlankiai", "Pakabos montavimo elementai, suvedimo varžtai", "Ratų guoliai",
                "Sankabos išminamieji guoliai", "Pakabos-važiuoklės įvorės", "Buksyravimo kilpos, kabliai",
                "Kilimėliai", "Autom.transmisijos filtrai", "Pavarų svirties užvalkalai", "Bagažinės kilimėliai",
                "Diržai trapeciniai", "Automobiliu_prekes", "Kiti automobilių aksesuarai, priedai", "Autoprekės",
                "AUTOMOBILIŲ PREKĖS"
            };
            return new List<string>(group);
        }

        public List<string> FurnitureGroup()//
        {
            var group = new List<string>();
            return group;
        }


        public List<string> HouseholdGroup()//
        {
            string[] group =
            {
                "Buities prekės", "Smulki buitinė technika", "Stambi buitinė technika",
                "Stalo servetėlės, vienkartinės staltiesės", "Kavavirės, kavamalės", "Vonios kambario stiklinės",
                "Vienkartinės servetėlės, nosinės, vatos gaminiai", "Dušo užuolaidos", "Kiti stalo serviravimo indai",
                "Svetainės, miegamojo staliniai šviestuvai", "Vienkartinės taurės, bokalai",
                "Tekstilinės servetėlės, staltiesės", "Šluostės", "Kavos, arbatos puodeliai",
                "Pramoniniai popieriaus gaminiai", "Dekoratyviniai buities elementai",
                "Reprodukcijos, paveikslai, sienų dekoracijos", "Termosai, gertuvės", "Kempinės, šluostės, grandikliai",
                "Puodeliai su lėkštute", "Pramoniniai laikikliai, dozatoriai", "Konditerijos įrankiai",
                "Virtuvės technika", "Virtuvės, buities, apyvokos prekės", "Baldai ir namų interjeras",
                "Santechnika, remontas, šildymas", "Buitinė technika ir elektronika", "Stambioji buitinė technika",
                "Montuojamoji buitinė technika", "Periferija, Biuro įranga", "Namų elektronika",
                "Buitinė technika. Kavos aparatai", "SANTECHNIKA, ŠILDYMAS", "BUITINĖ TECHNIKA IR ELEKTRONIKA",
                "LEMPOS IR APŠVIETIMAS", "BALDAI IR NAMŲ INTERJERAS", "BUITIES, VIRTUVĖS, APYVOKOS PREKĖS"
            };
            return new List<string>(group);
        }

        public List<string> AudioGroup()//
        {
            string[] group =
            {
                "Vaizdo ir garso technika", "Garso ir vaizdo technika"
            };
            return new List<string>(group);
        }

        public List<string> AnimalsGroup()//
        {
            string[] group =
                {"Gyvūnų prekės", "PREKĖS GYVŪNAMS"};
            return new List<string>(group);
        }

        public List<string> BeautyGroup()//
        {
            string[] group =
            {
                "Sveikata, grožis ir laisvalaikis", "Kvepalai, kosmetika", "Grožis ir sveikata", "KVEPALAI, KOSMETIKA"
            };
            return new List<string>(group);
        }

        public List<string> CameraGroup()//
        {
            string[] group = {"Vaizdo ir garso technika", "Mobilieji telefonai, Foto ir Video"};
            return new List<string>(group);
        }

        public List<string> StationeryGroup()//
        {
            string[] group =
            {
                "tapybos"
            };
            return new List<string>(group);
        }

        public List<string> BooksGroup()//
        {
            string[] group = {"Knygos", "KNYGOS, BIURO PREKĖS"};
            return new List<string>(group);
        }

        public List<string> ComputerGroup()//
        {
            string[] group =
            {
                "Kompiuteriai ir jų komponentai", "Vaizdo ir garso technika", "Kompiuteriniai žaidimai",
                "Nešiojami kompiuteriai", "Kompiuterių krepšiai, kuprinės, dėklai", "Kompiuterių priedai",
                "Žaidimų kompiuteriai ir žaidimai", "Kiti kompiuterių aksesuarai",
                "Kompiuteriai, telefonai, IT", "Kompiuterinė technika", "Televizoriai",
                "Žaidimų kompiuteriai ir jų priedai", "Kompiuteriai, Komponentai", "Žaidimų įranga, žaidimai",
                "Telefonai ir planšetiniai kompiuteriai", "KOMPIUTERINĖ TECHNIKA"
            };
            return new List<string>(group);
        }


        public List<string> LeisureGroup()//
        {
            string[] group =
            {
                "Laisvalaikio prekės", "Gimtadienio atributika", "Sportas, laisvalaikis, turizmas",
                "Riedžiai ir paspirtukai", "SPORTAS, LAISVALAIKIS, TURIZMAS"
            };
            return new List<string>(group);
        }

        public List<string> MaistasGroup()//
        {
            string[] group = { };
            return new List<string>(group);
        }

        public List<string> GardenGroup()//
        {
            string[] group =
            {
                "Kiti įrankiai", "Apskaitos skydai, skydeliai", "Šlifavimo diskai", "Diskiniai pjūklai",
                "Elektrinių įrankių dalys, priedai", "Elektriniai atsuktuvai", "Sodo prekės",
                "Dovanos, šventinė atributika", "Apsaugos, dezinfekcinės priemonės", "SODO PREKĖS, ĮRANKIAI"
            };
            return new List<string>(group);
        }

        public List<string> PhoneGroup()//
        {
            string[] group =
            {
                "Mobilūs telefonai", "Mobilieji telefonai, Foto ir Video", "Mobilieji telefonai ir jų aksesuarai",
                "Komunikacinė ir ryšio įranga", "Telefonai, Išmanieji pagalbininkai",
                "Telefonai ir planšetiniai kompiuteriai", "MOBILIEJI TELEFONAI, FOTO IR VIDEO"
            };
            return new List<string>(group);
        }
    }
}
//,"None"