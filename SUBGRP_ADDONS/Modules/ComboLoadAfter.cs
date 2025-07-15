using SAPbouiCOM.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SUBGRP_ADDONS.Helper;
using SUBGRP_ADDONS.Resources;

namespace SUBGRP_ADDONS.Modules
{
    class ComboLoadAfter
    {
        public ComboLoadAfter()
        {
            Application.SBO_Application.ItemEvent += new SAPbouiCOM._IApplicationEvents_ItemEventEventHandler(SBO_Application_ItemEvent);
        }

        private void SBO_Application_ItemEvent(string FormUID, ref SAPbouiCOM.ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            if (pVal.FormTypeEx == "9999" && pVal.ItemUID == "1" &&
    pVal.EventType == SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED 
    )
            {
                try
                {
                    // Make sure FRMSBGRP is open
                    SAPbouiCOM.Form ofrm;
                    try
                    {
                        ofrm = Application.SBO_Application.Forms.Item("FRMSBGRP");
                    }
                    catch
                    {
                        Application.SBO_Application.StatusBar.SetText("FRMSBGRP form is not open.", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
                        return;
                    }

                    if (ofrm.Mode == SAPbouiCOM.BoFormMode.fm_OK_MODE || ofrm.Mode == SAPbouiCOM.BoFormMode.fm_FIND_MODE)
                    {
                        //ofrm.Select(); // Bring FRMSBGRP to focus
                        ofrm.Freeze(true);

                        // Populate Series ComboBox
                        SAPbouiCOM.ComboBox CBSERISE = (SAPbouiCOM.ComboBox)ofrm.Items.Item("CBSERISE").Specific;
                      
                        // Get selected value from CBSERISE
                        string selectedValue = CBSERISE.Value;
                        if (string.IsNullOrEmpty(selectedValue))
                        {
                            // ── Get value from Form 9999 Matrix ──
                            SAPbouiCOM.Form frm9999 = Application.SBO_Application.Forms.Item(pVal.FormUID);
                            SAPbouiCOM.Matrix oMatrix = (SAPbouiCOM.Matrix)frm9999.Items.Item("7").Specific;

                            int rowSelected = oMatrix.GetNextSelectedRow(0, SAPbouiCOM.BoOrderType.ot_RowOrder);

                            if (rowSelected > 0)
                            {
                                selectedValue = ((SAPbouiCOM.EditText)oMatrix.Columns.Item("U_SERIES").Cells.Item(rowSelected).Specific).Value;
                            }
                        }

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
                                Application.SBO_Application.StatusBar.SetText("Invalid Series value. Must be a number.", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
                            }
                        }

                        ofrm.Freeze(false);
                    }
                }
                catch (Exception ex)
                {
                    Application.SBO_Application.StatusBar.SetText("Error after button press: " + ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
                }
            }




            if (pVal.FormTypeEx == "1473000001" &&
    pVal.ItemUID == "1470000014" &&
    pVal.ColUID == "254000156" &&
    pVal.EventType == SAPbouiCOM.BoEventTypes.et_CLICK )
            {
                SAPbouiCOM.Form oForm = Application.SBO_Application.Forms.Item(pVal.FormUID);
                SAPbouiCOM.Matrix oMatrix = (SAPbouiCOM.Matrix)oForm.Items.Item("1470000014").Specific;
                SAPbouiCOM.Column oColumn = oMatrix.Columns.Item("254000156");

                oForm.Freeze(true);

                try
                {
                    // Only clear and add ValidValues if they aren't already loaded
                    if (oColumn.ValidValues.Count <= 1)
                    {
                        for (int i = oColumn.ValidValues.Count - 1; i >= 0; i--)
                        {
                            oColumn.ValidValues.Remove(i, SAPbouiCOM.BoSearchKey.psk_Index);
                        }

                        SAPbobsCOM.Recordset rSet = (SAPbobsCOM.Recordset)Global.oComp.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                        string qStr = @"SELECT ""Code"", ""Descr"" FROM ""OFYM""";
                        rSet.DoQuery(qStr);

                        while (!rSet.EoF)
                        {
                            oColumn.ValidValues.Add(rSet.Fields.Item("Code").Value.ToString(), rSet.Fields.Item("Descr").Value.ToString());
                            rSet.MoveNext();
                        }
                    }

                    // Optionally: Set cell value in the clicked row
                    int clickedRow = pVal.Row;
                    oMatrix.SetCellWithoutValidation(clickedRow, "254000156", "-");

                }
                catch (Exception ex)
                {
                    Application.SBO_Application.StatusBar.SetText("Error: " + ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
                }
                finally
                {
                    oForm.Freeze(false);
                }
            }





        }
    }
}
