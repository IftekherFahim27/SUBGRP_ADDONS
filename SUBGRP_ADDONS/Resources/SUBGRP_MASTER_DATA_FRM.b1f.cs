using SAPbouiCOM.Framework;
using SUBGRP_ADDONS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SUBGRP_ADDONS.Resources
{
    [FormAttribute("SUBGRP_ADDONS.Resources.SUBGRP_MASTER_DATA_FRM", "Resources/SUBGRP_MASTER_DATA_FRM.b1f")]
    class SUBGRP_MASTER_DATA_FRM : UserFormBase
    {
        public SUBGRP_MASTER_DATA_FRM()
        {
        }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.STSERISE = ((SAPbouiCOM.StaticText)(this.GetItem("STSERISE").Specific));
            this.CBSERISE = ((SAPbouiCOM.ComboBox)(this.GetItem("CBSERISE").Specific));
            this.CBSERISE.ComboSelectAfter += new SAPbouiCOM._IComboBoxEvents_ComboSelectAfterEventHandler(this.CBSERISE_ComboSelectAfter);
            this.STIGRCOD = ((SAPbouiCOM.StaticText)(this.GetItem("STIGRCOD").Specific));
            this.CBIGRCOD = ((SAPbouiCOM.ComboBox)(this.GetItem("CBIGRCOD").Specific));
            this.CBIGRCOD.ComboSelectAfter += new SAPbouiCOM._IComboBoxEvents_ComboSelectAfterEventHandler(this.CBIGRCOD_ComboSelectAfter);
            this.STSGRCOD = ((SAPbouiCOM.StaticText)(this.GetItem("STSGRCOD").Specific));
            this.ETSGRCOD = ((SAPbouiCOM.EditText)(this.GetItem("ETSGRCOD").Specific));
            this.ETSGRNAM = ((SAPbouiCOM.EditText)(this.GetItem("ETSGRNAM").Specific));
            this.STSGRNAM = ((SAPbouiCOM.StaticText)(this.GetItem("STSGRNAM").Specific));
            this.ADDButton = ((SAPbouiCOM.Button)(this.GetItem("1").Specific));
            this.CancelButton = ((SAPbouiCOM.Button)(this.GetItem("2").Specific));
            this.EditText2 = ((SAPbouiCOM.EditText)(this.GetItem("ETDNTRY").Specific));
            this.OnCustomInitialize();

        }

        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
        }

        private SAPbouiCOM.StaticText STSERISE;

        private void OnCustomInitialize()
        {

        }

        private SAPbouiCOM.ComboBox CBSERISE;
        private SAPbouiCOM.StaticText STIGRCOD;
        private SAPbouiCOM.ComboBox CBIGRCOD;
        private SAPbouiCOM.StaticText STSGRCOD;
        private SAPbouiCOM.EditText ETSGRCOD;
        private SAPbouiCOM.EditText ETSGRNAM;
        private SAPbouiCOM.StaticText STSGRNAM;
        private SAPbouiCOM.Button ADDButton;
        private SAPbouiCOM.Button CancelButton;
        private SAPbouiCOM.EditText EditText2;

        private void CBSERISE_ComboSelectAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            SAPbouiCOM.Form oForm = Application.SBO_Application.Forms.Item(pVal.FormUID);
            SAPbouiCOM.ComboBox oCombo = (SAPbouiCOM.ComboBox)oForm.Items.Item("CBSERISE").Specific;

            string selectedValue = oCombo.Value;
            int series = int.Parse(selectedValue);      // Converts to int

            oForm.Freeze(true);

            //Item Group Code 
            string sqlQuery = string.Format(@" SELECT DISTINCT OITM.{0}ItmsGrpCod{0}, OITB.{0}ItmsGrpNam{0} FROM OITM INNER JOIN OITB ON OITM.{0}ItmsGrpCod{0} = OITB.{0}ItmsGrpCod{0} WHERE  OITM.{0}Series{0} = {1}", '"', series);
            SAPbouiCOM.ComboBox CBIGRCOD = (SAPbouiCOM.ComboBox)oForm.Items.Item("CBIGRCOD").Specific;   //object defining- Define a combo box
            Global.GFunc.setComboBoxValue(CBIGRCOD, sqlQuery);

            oForm.Freeze(false);
        }

        private void CBIGRCOD_ComboSelectAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            SAPbouiCOM.Form oForm = Application.SBO_Application.Forms.Item(pVal.FormUID);
            SAPbouiCOM.ComboBox oCombo = (SAPbouiCOM.ComboBox)oForm.Items.Item("CBIGRCOD").Specific;

            string selectedValue = oCombo.Value;
            int series = int.Parse(selectedValue);



        }
    }
}
