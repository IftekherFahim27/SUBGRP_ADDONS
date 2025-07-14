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
            this.ADDButton.PressedAfter += new SAPbouiCOM._IButtonEvents_PressedAfterEventHandler(this.ADDButton_PressedAfter);
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

            if (oForm.Mode == SAPbouiCOM.BoFormMode.fm_UPDATE_MODE || oForm.Mode == SAPbouiCOM.BoFormMode.fm_ADD_MODE || oForm.Mode == SAPbouiCOM.BoFormMode.fm_OK_MODE)
            {
                try
                {
                    SAPbouiCOM.ComboBox oCombo = (SAPbouiCOM.ComboBox)oForm.Items.Item("CBSERISE").Specific;
                    string selectedValue = oCombo.Value;

                    oForm.Freeze(true);

                    SAPbouiCOM.ComboBox CBIGRCOD = (SAPbouiCOM.ComboBox)oForm.Items.Item("CBIGRCOD").Specific;

                    // Clear existing values
                    for (int i = CBIGRCOD.ValidValues.Count - 1; i >= 0; i--)
                    {
                        CBIGRCOD.ValidValues.Remove(i, SAPbouiCOM.BoSearchKey.psk_Index);
                    }

                    // Add default value
                    CBIGRCOD.ValidValues.Add("-", "Select");
                    CBIGRCOD.Select("-", SAPbouiCOM.BoSearchKey.psk_ByValue);

                    if (!string.IsNullOrEmpty(selectedValue))
                    {
                        if (int.TryParse(selectedValue, out int seriesNumeric))
                        {
                            string sqlQuery = $@"SELECT ""ItmsGrpCod"", ""ItmsGrpNam"" FROM ""OITB"" WHERE ""U_SERIES"" = {seriesNumeric}";
                            SAPbouiCOM.ComboBox comboitem = (SAPbouiCOM.ComboBox)oForm.Items.Item("CBIGRCOD").Specific;
                            Global.GFunc.setComboBoxValue(comboitem, sqlQuery);
                        }
                        else
                        {
                            Application.SBO_Application.StatusBar.SetText("Invalid Series value. Must be a number.",
                                SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Application.SBO_Application.MessageBox("Error in CBSERISE_ComboSelectAfter: " + ex.Message);
                }
                finally
                {
                    oForm.Freeze(false);
                }
            }
        }


       

        private void CBIGRCOD_ComboSelectAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            try
            {
                SAPbouiCOM.Form oForm = Application.SBO_Application.Forms.Item(pVal.FormUID);

                if (oForm.Mode == SAPbouiCOM.BoFormMode.fm_UPDATE_MODE || oForm.Mode == SAPbouiCOM.BoFormMode.fm_ADD_MODE)
                {
                    int nextCode = 101; // Default value
                    string query = $@"SELECT IFNULL(MAX(CAST(""Code"" AS INTEGER)), 0) AS MaxCode FROM ""@FIL_MH_SUBGRP"" ";

                    SAPbobsCOM.Recordset rs = (SAPbobsCOM.Recordset)Global.oComp.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                    rs.DoQuery(query);

                    int maxCode = Convert.ToInt32(rs.Fields.Item("MaxCode").Value);

                    if (maxCode >= 101)
                    {
                        nextCode = maxCode + 1;
                    }

                    SAPbouiCOM.EditText oEdit = (SAPbouiCOM.EditText)oForm.Items.Item("ETSGRCOD").Specific;
                    oEdit.Value = nextCode.ToString();

                }
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

        private void ADDButton_PressedAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            SAPbouiCOM.Form ofrm = Application.SBO_Application.Forms.Item(pVal.FormUID);

            try
            {
                if (ofrm.Mode == SAPbouiCOM.BoFormMode.fm_FIND_MODE)
                {
                    ofrm.Items.Item("ETSGRCOD").Enabled = false;
                    return; // No need to proceed further if in Find Mode
                }

                if (ofrm.Mode == SAPbouiCOM.BoFormMode.fm_OK_MODE)
                {
                    ofrm.Freeze(true);

                    // Populate Series ComboBox
                    string sqlQuerySeries = @"SELECT ""Series"", ""SeriesName"" FROM ""NNM1"" WHERE ""ObjectCode"" = '4'";
                    SAPbouiCOM.ComboBox CBSERISE = (SAPbouiCOM.ComboBox)ofrm.Items.Item("CBSERISE").Specific;
                    Global.GFunc.setComboBoxValue(CBSERISE, sqlQuerySeries);

                    // Get selected value from CBSERISE
                    string selectedValue = CBSERISE.Value;

                    if (!string.IsNullOrEmpty(selectedValue))
                    {
                        if (int.TryParse(selectedValue, out int seriesNumeric))
                        {
                            string sqlQuery = $@"SELECT ""ItmsGrpCod"", ""ItmsGrpNam"" FROM ""OITB"" WHERE ""U_SERIES"" = {seriesNumeric}";
                            SAPbouiCOM.ComboBox CBIGRCOD = (SAPbouiCOM.ComboBox)ofrm.Items.Item("CBIGRCOD").Specific;
                            Global.GFunc.setComboBoxValue(CBIGRCOD, sqlQuery);
                        }
                        else
                        {
                            Application.SBO_Application.StatusBar.SetText("Invalid Series value. Must be a number.",
                                SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Application.SBO_Application.MessageBox("Error in ADDButton_PressedAfter: " + ex.Message);
            }
            finally
            {
                ofrm.Freeze(false);
            }
        }

    }
}
