using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LV_Kommunikation {
    class LVCommunicator
    {
        public string exe;

        private LVManage8.cLVSupport lvSupport;
        private LVManage8.cAtlas atlas;



        public LVCommunicator() {
        }
        public LVCommunicator(string exe)
        {
            StartAtlas(exe);
        }

        public bool StartAtlas(string exe) {
            if(lvSupport == null) {
                this.exe = exe;
                lvSupport = new LVManage8.cLVSupport();
                if(atlas == null) {
                    atlas = lvSupport.getObject(exe); //Starts Lauterbach or gets the running instance
                    if (atlas == null) {
                        atlas = lvSupport.Create(exe);
                    }
                    
                    
                    //short nummer = atlas.CreateModul("");
                    //atlas.activateModulNr(nummer);
                    //atlas.Show(true);
                }
            }
            if (atlas != null) {
                return true;
            }
            else {
                return false;
            }

        }
        ~LVCommunicator()
        {
            atlas.Quit();
        }

        public void Shutdown() {
            if(atlas != null) {
                atlas.Quit();
            }
            
        }

        /// <summary>
        /// kap = modul number
        /// var = variable number in Lauterbach
        /// k = alternativ calculation
        /// </summary>
        /// <param name="kap">Module number ( e.g. 1 B1 ---> kap = 1)</param>
        /// <param name="var"> Variable Number (seen in the GUI at the bottom, while variable box is selected.</param>
        /// <param name="value"></param>
        public void SetValue(int kap, int var, object input)
        {
            short Chapter = Convert.ToInt16(kap);
            short VariableNummer = Convert.ToInt16(var);
   

            atlas.activateModulNr(Chapter);
            atlas.set_Inp(VariableNummer, input);
            atlas.Atlasbek[Chapter, VariableNummer, 1] = 1; /*BEK = 1 user input
                                                                       *BEK = 0 var unknown
                                                                       *BEK > 0 index of combobox
                                                                       *BEK < 0 calculated */

            atlas.ModulRefresh(true);
        }
       
        /// <summary>
        /// s. SetValue
        /// </summary>
        /// <param name="kap"></param>
        /// <param name="var"></param>
        /// <returns></returns>
        public double GetValue(int kap, int var)
        {
            short Chapter = Convert.ToInt16(kap);
            short VariableNummer = Convert.ToInt16(var);
            
            return Convert.ToDouble(atlas.AtlasWert[Chapter, VariableNummer, 1]);
        }

        public short StartModule(string module)
        {
            short nummer = atlas.CreateModul(module);
            atlas.activateModulNr(nummer);
            atlas.Show(true);
            return nummer;
        }

        public void ActivateChapter(short kap) {
            short Chapter = Convert.ToInt16(kap);
            atlas.activateModulNr(Chapter);
            atlas.ModulRefresh();
        }

        public void Hide() {
            atlas.Hide();
        }

        public void Show() {
            atlas.Show();
        }

        public int GetBek(int kap, int var) {

            short Chapter = Convert.ToInt16(kap);
            short VariableNummer = Convert.ToInt16(var);

            return (int) atlas.Atlasbek[Chapter, VariableNummer, 1];
        }

        public void StartB1WithPresettings() {
            //AD ist das Projekt-Modul in dem Norm, Werkstoffdefinition und Lastfall Betrachtung oder nicht vorgegeben wird (wichtig für alle folgenden Festigkeitsmodule)
            string module = "AD"; 
            short kap = atlas.CreateModul(module);
            //es gibt 10 CalcTypes (für jedes Modul, wovon jedoch nicht immer alle verwendet werden), diese verursachen die Vorauswahl-Form beim start eines Moduls
            //für AD sind dies die auswahlen die zu treffen sind:
            atlas.CalcType[2, kap] = 2; // Typ 2 = ohne Lastfälle, Typ 3 = mit Lastfälle
            atlas.CalcType[3, kap] = 1; //Norm AD2000 in Projekt-Modul(AD) --> 2: EN 13445 ... --> kann später über die Combobox mit Variablennummer 269 noch geändert werden
            atlas.CalcType[4, kap] = 1; //Werkstoffdefinition nach  AD2000 in Projekt-Modul(AD) --> 2: EN 13445 , 3 ...  --> selbe wie oben hier mit 270
            atlas.activateModulNr(kap);

            //jetzt kann B1 und jedes weitere Festigkeitsmodul gestartet werden
            module = "B1";
            kap = atlas.CreateModul(module);
            atlas.CalcType[1, kap] = 1; //1: Zylinderschalen; 2: Kugelschalen
            atlas.activateModulNr(kap);
            //jetzt sind die notwendigen Eingaben erledigt. Drücken Sie auf den Hide/Show-Button um sich das ergebnis anzusehen

        }

    }
}
