using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAPbouiCOM.Framework;
using SUBGRP_ADDONS.Helper;

namespace SUBGRP_ADDONS.Modules
{
    class ItemGroups
    {
        public ItemGroups()
        {
            Application.SBO_Application.ItemEvent += new SAPbouiCOM._IApplicationEvents_ItemEventEventHandler(SBO_Application_ItemEvent);
        
        }

        private static bool flag = false;

        private void SBO_Application_ItemEvent(string FormUID, ref SAPbouiCOM.ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            try
            {
                if (pVal.FormTypeEx == "63" && pVal.EventType != SAPbouiCOM.BoEventTypes.et_FORM_UNLOAD)
                {
                    //define a form in 3 ways 
                    SAPbouiCOM.Form oform = Application.SBO_Application.Forms.GetFormByTypeAndCount(pVal.FormType, pVal.FormTypeCount);

                    if (pVal.EventType == SAPbouiCOM.BoEventTypes.et_FORM_LOAD && pVal.BeforeAction == true)
                    {
                        //Series ST
                        SAPbouiCOM.Item STSERIES = oform.Items.Add("STSERIES", SAPbouiCOM.BoFormItemTypes.it_STATIC); // we are going to create
                        SAPbouiCOM.Item osrc = oform.Items.Item("4");
                        STSERIES.Top = osrc.Top + 15;
                        STSERIES.Height = osrc.Height;
                        STSERIES.Width = osrc.Width;
                        STSERIES.Left = osrc.Left;

                        SAPbouiCOM.StaticText Series = ((SAPbouiCOM.StaticText)(STSERIES.Specific));
                        Series.Caption = "Series";

                        //Series ET
                        SAPbouiCOM.Item CBSERIES = oform.Items.Add("CBSERIES", SAPbouiCOM.BoFormItemTypes.it_COMBO_BOX); // we are going to create
                        SAPbouiCOM.Item orsc1 = oform.Items.Item("6");
                        CBSERIES.Top = STSERIES.Top;
                        CBSERIES.Height = orsc1.Height;
                        CBSERIES.Width = orsc1.Width;
                        CBSERIES.Left = orsc1.Left;

                        string sqlQuerySeries = string.Format("SELECT {0}Series{0}, {0}SeriesName{0} FROM {0}NNM1{0} WHERE {0}ObjectCode{0} = '4'", '"');
                        SAPbouiCOM.ComboBox CBSERISE = (SAPbouiCOM.ComboBox)oform.Items.Item("CBSERIES").Specific;   //object defining- Define a combo box
                        Global.GFunc.setComboBoxValue(CBSERISE, sqlQuerySeries);



                        CBSERISE.DataBind.SetBound(true, "OITB", "U_SERIES"); //TO SAVE THE VALUE IN TABLE

                    }

                }
                if (pVal.FormTypeEx == "150" && pVal.EventType != SAPbouiCOM.BoEventTypes.et_FORM_UNLOAD)
                {
                    //define a form in 3 ways 
                    SAPbouiCOM.Form oform = Application.SBO_Application.Forms.GetFormByTypeAndCount(pVal.FormType, pVal.FormTypeCount);

                    if (pVal.EventType == SAPbouiCOM.BoEventTypes.et_FORM_LOAD && pVal.BeforeAction == true)
                    {
                        //Series ST
                        SAPbouiCOM.Item STSUBIGRP = oform.Items.Add("STSUBIGRP", SAPbouiCOM.BoFormItemTypes.it_STATIC); // we are going to create
                        SAPbouiCOM.Item osrc = oform.Items.Item("39");
                        STSUBIGRP.Top = osrc.Top;
                        STSUBIGRP.Height = osrc.Height;
                        STSUBIGRP.Width = osrc.Width;
                        STSUBIGRP.Left = osrc.Left + osrc.Width + 20;

                        SAPbouiCOM.StaticText Series = ((SAPbouiCOM.StaticText)(STSUBIGRP.Specific));
                        Series.Caption = "Sub Item Group ";

                        //Series CB
                        SAPbouiCOM.Item CBSUIGRP = oform.Items.Add("CBSUIGRP", SAPbouiCOM.BoFormItemTypes.it_COMBO_BOX); // we are going to create
                        SAPbouiCOM.Item orsc1 = oform.Items.Item("STSUBIGRP");
                        CBSUIGRP.Top = STSUBIGRP.Top;
                        CBSUIGRP.Height = orsc1.Height;
                        CBSUIGRP.Width = orsc1.Width;
                        CBSUIGRP.Left = orsc1.Left + orsc1.Width;
                        CBSUIGRP.LinkTo = "STSUBIGRP";

                       
                        SAPbouiCOM.ComboBox ComBSUIGRP = (SAPbouiCOM.ComboBox)oform.Items.Item("CBSUIGRP").Specific;   //object defining- Define a combo box
                        ComBSUIGRP.DataBind.SetBound(true, "OITM", "U_SUBGRCOD"); //TO SAVE THE VALUE IN TABLE


                        //Itemgroup ST
                        SAPbouiCOM.Item STITMGRP = oform.Items.Add("STITMGRP", SAPbouiCOM.BoFormItemTypes.it_STATIC); // we are going to create
                        SAPbouiCOM.Item ostsubgrp = oform.Items.Item("STSUBIGRP");
                        STITMGRP.Top = ostsubgrp.Top - (ostsubgrp.Height+2);
                        STITMGRP.Height = ostsubgrp.Height;
                        STITMGRP.Width = ostsubgrp.Width;
                        STITMGRP.Left = ostsubgrp.Left;

                        SAPbouiCOM.StaticText IGRP = ((SAPbouiCOM.StaticText)(STITMGRP.Specific));
                        IGRP.Caption = "Item Group ";

                        //Item Group Combobox
                        SAPbouiCOM.Item CBITMGRP = oform.Items.Add("CBITMGRP", SAPbouiCOM.BoFormItemTypes.it_COMBO_BOX); // we are going to create
                        SAPbouiCOM.Item ocbsubgrp = oform.Items.Item("CBSUIGRP");
                        CBITMGRP.Top = ocbsubgrp.Top - (ocbsubgrp.Height+2);
                        CBITMGRP.Height = ocbsubgrp.Height;
                        CBITMGRP.Width = ocbsubgrp.Width;
                        CBITMGRP.Left = ocbsubgrp.Left;
                        CBITMGRP.LinkTo = "STITMGRP";




                    }
                    if (pVal.FormTypeEx == "150" && pVal.ItemUID == "39" && pVal.EventType == SAPbouiCOM.BoEventTypes.et_COMBO_SELECT && pVal.BeforeAction == false)
                    {
                        SAPbouiCOM.ComboBox ComBSUIGRP = (SAPbouiCOM.ComboBox)oform.Items.Item("39").Specific;
                        string selectedValue = ComBSUIGRP.Selected.Value;

                        // Build query to get sub groups for selected U_ITMGRCOD
                        string sqlQuerySubGrp = string.Format("SELECT {0}Code{0}, {0}Name{0} FROM {0}@FIL_MH_SUBGRP{0} WHERE {0}U_ITMGRCOD{0} = '{1}'",'"', selectedValue);

                        SAPbouiCOM.ComboBox ComBoSUIGRP = (SAPbouiCOM.ComboBox)oform.Items.Item("CBSUIGRP").Specific;
                        Global.GFunc.setComboBoxValue(ComBoSUIGRP, sqlQuerySubGrp);
                    }

                    if (pVal.FormTypeEx == "150" && pVal.ItemUID == "1320002059" && pVal.EventType == SAPbouiCOM.BoEventTypes.et_COMBO_SELECT && pVal.BeforeAction == false)
                    {
                        SAPbouiCOM.ComboBox CMBSeries = (SAPbouiCOM.ComboBox)oform.Items.Item("1320002059").Specific;
                        string selectedValue = CMBSeries.Selected.Value;
                        int series = int.Parse(selectedValue);

                        // Build query to get sub groups for selected U_ITMGRCOD
                        string sqlQuerySubGrp = string.Format("SELECT {0}ItmsGrpCod{0}, {0}ItmsGrpNam{0} FROM {0}OITB{0} WHERE {0}U_SERIES{0} = '{1}'", '"', selectedValue);

                        SAPbouiCOM.ComboBox ComBoIGRP = (SAPbouiCOM.ComboBox)oform.Items.Item("CBITMGRP").Specific;

                        // 🧹 Clear existing combo values
                        for (int i = ComBoIGRP.ValidValues.Count - 1; i >= 0; i--)
                        {
                            ComBoIGRP.ValidValues.Remove(i, SAPbouiCOM.BoSearchKey.psk_Index);
                        }

                        // ➕ Add default item at top: "Select"
                        ComBoIGRP.ValidValues.Add("0", "Select");
                        ComBoIGRP.Select("0", SAPbouiCOM.BoSearchKey.psk_ByValue);

                        Global.GFunc.setComboBoxValue(ComBoIGRP, sqlQuerySubGrp);
                    }


                    if (pVal.FormTypeEx == "150" && pVal.ItemUID == "CBITMGRP" && pVal.EventType == SAPbouiCOM.BoEventTypes.et_COMBO_SELECT && pVal.BeforeAction == false)
                    {
                        SAPbouiCOM.ComboBox CBIGRP = (SAPbouiCOM.ComboBox)oform.Items.Item("CBITMGRP").Specific;
                        string selectedValue = CBIGRP.Selected.Value;
                       

                        //standard

                        SAPbouiCOM.ComboBox ComBoSIGRP = (SAPbouiCOM.ComboBox)oform.Items.Item("39").Specific;                       
                        ComBoSIGRP.Select(selectedValue, SAPbouiCOM.BoSearchKey.psk_ByValue);

                        
                    }






                }
            }
            catch (Exception ex)
            {
                Application.SBO_Application.SetStatusBarMessage("Error in Itemevnt for SAP Screen - " + ex.ToString(), SAPbouiCOM.BoMessageTime.bmt_Medium, true);
            }
        }

    }
}
