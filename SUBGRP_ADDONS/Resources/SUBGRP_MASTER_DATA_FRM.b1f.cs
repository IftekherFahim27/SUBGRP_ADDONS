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
            this.ADDButton.PressedBefore += new SAPbouiCOM._IButtonEvents_PressedBeforeEventHandler(this.ADDButton_PressedBefore);
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
            //string sqlQuery = string.Format(@" SELECT DISTINCT OITM.{0}ItmsGrpCod{0}, OITB.{0}ItmsGrpNam{0} FROM OITM INNER JOIN OITB ON OITM.{0}ItmsGrpCod{0} = OITB.{0}ItmsGrpCod{0} WHERE  OITM.{0}Series{0} = {1}", '"', series);
            string sqlQuery2 = string.Format(@" SELECT {0}ItmsGrpCod{0}, {0}ItmsGrpNam{0} FROM OITB WHERE  {0}U_SERIES{0} = {1}", '"', series);
            SAPbouiCOM.ComboBox CBIGRCOD = (SAPbouiCOM.ComboBox)oForm.Items.Item("CBIGRCOD").Specific;   //object defining- Define a combo box

            // 🧹 Clear existing combo values
            for (int i = CBIGRCOD.ValidValues.Count - 1; i >= 0; i--)
            {
                CBIGRCOD.ValidValues.Remove(i, SAPbouiCOM.BoSearchKey.psk_Index);
            }

            // ➕ Add default item at top: "Select"
            CBIGRCOD.ValidValues.Add("0", "Select");
            CBIGRCOD.Select("0", SAPbouiCOM.BoSearchKey.psk_ByValue);

            // 🔁 Populate from SQL
            Global.GFunc.setComboBoxValue(CBIGRCOD, sqlQuery2);
           

            oForm.Freeze(false);
        }

        private void CBIGRCOD_ComboSelectAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            try
            {
                SAPbouiCOM.Form oForm = Application.SBO_Application.Forms.Item(pVal.FormUID);
                SAPbouiCOM.ComboBox oCombo = (SAPbouiCOM.ComboBox)oForm.Items.Item("CBIGRCOD").Specific;
                string selectedValue = oCombo.Value; // U_ITMGRCOD selected
                string nextCode = "SubGrp 1"; // Default if no match found

                // SQL to get max SubGrp number for the selected U_ITMGRCOD
                string query = $@"
            SELECT MAX(CAST(SUBSTRING(""Code"", 8) AS INTEGER)) AS MaxNum 
            FROM ""@FIL_MH_SUBGRP"" 
            WHERE ""U_ITMGRCOD"" = '{selectedValue}' 
              AND ""Code"" LIKE 'SubGrp %'";

                SAPbobsCOM.Recordset rs = (SAPbobsCOM.Recordset)Global.oComp.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                rs.DoQuery(query);

                if (!rs.EoF && rs.Fields.Item("MaxNum").Value != null && rs.Fields.Item("MaxNum").Value.ToString() != "")
                {
                    int maxNum = Convert.ToInt32(rs.Fields.Item("MaxNum").Value);
                    nextCode = "SubGrp " + (maxNum + 1);
                }

                // Set nextCode in the EditText bound to "Code"
                SAPbouiCOM.EditText oEdit = (SAPbouiCOM.EditText)oForm.Items.Item("ETSGRCOD").Specific;
                oEdit.Value = nextCode;
            }
            catch (Exception ex)
            {
                Application.SBO_Application.StatusBar.SetText("Error: " + ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
            }
        }

        private void ADDButton_PressedBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            SAPbouiCOM.Form oform = Application.SBO_Application.Forms.Item(pVal.FormUID);
            if (oform.Mode == SAPbouiCOM.BoFormMode.fm_ADD_MODE || oform.Mode == SAPbouiCOM.BoFormMode.fm_UPDATE_MODE)
            {
                ValidateForm(ref oform, ref BubbleEvent);
            }

        }

        private bool ValidateForm(ref SAPbouiCOM.Form pForm, ref bool BubbleEvent)
        {
            string SubGrpName = pForm.DataSources.DBDataSources.Item("@FIL_MH_SUBGRP").GetValue("Name", 0);
            string Series = pForm.DataSources.DBDataSources.Item("@FIL_MH_SUBGRP").GetValue("U_SERIES", 0);
            string ItemGrp = pForm.DataSources.DBDataSources.Item("@FIL_MH_SUBGRP").GetValue("U_ITMGRCOD", 0);


            if (Series == "")
            {
                Global.GFunc.ShowError("Select Series");
                pForm.ActiveItem = "CBSERISE";
                return BubbleEvent = false;
            }

            if (ItemGrp == "" || ItemGrp == "0")
            {
                Global.GFunc.ShowError("Select Item Group ");
                pForm.ActiveItem = "ETSGRCOD";
                return BubbleEvent = false;
            }

            if (SubGrpName == "")
            {
                Global.GFunc.ShowError("Enter Sub Group Name");
                pForm.ActiveItem = "ETSGRNAM";
                return BubbleEvent = false;
            }


            return BubbleEvent;
        }

    }
}
