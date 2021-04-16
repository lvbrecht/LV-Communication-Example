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

        public void StartAtlas(string exe) {
            if(lvSupport == null) {
                this.exe = exe;
                lvSupport = new LVManage8.cLVSupport();
                if(atlas == null) {
                    atlas = lvSupport.getObject(exe); //Starts Lauterbach or gets the running instance
                    if (atlas == null) {
                        atlas = lvSupport.Create(exe);
                    }
                    short nummer = atlas.CreateModul("");
                    atlas.activateModulNr(nummer);
                    atlas.Show(true);
                }
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
            atlas.Show(true);
        }

        public int GetBek(int kap, int var) {

            short Chapter = Convert.ToInt16(kap);
            short VariableNummer = Convert.ToInt16(var);

            return (int) atlas.Atlasbek[Chapter, VariableNummer, 1];
        }

    }
}
