# SUBGRP_ADDONS – SAP Business One Add-On

## Overview

This is a custom SAP Business One (SAP B1) UI API add-on developed using SAPbouiCOM and SAPbobsCOM SDK libraries.  
The primary purpose is to enhance Item Master Data management by implementing cascading filtering logic between:

- **Item Series**
- **Item Group**
- **Item Sub-Group**

The add-on includes form-level customization, matrix handling, and dynamic combo box population based on business rules described below.

---

## Core Features

1. **Cascading Selection Logic**

    - **Step 1:** User selects an Item Series → Item Groups get filtered by that series.
    - **Step 2:** User selects an Item Group → Item Sub-Groups get filtered by that group.
    - **Step 3:** If Item Sub-Group is updated in the Sub-Group Master form, the change reflects automatically in Item Master Data.

2. **Custom Forms and Events**
    - Sub Group Master Data Form (`FRMSBGRP`)
    - Integration with SAP standard Item Master Data form (`150`)
    - Custom Choose From List (CFL) for Item Sub-Group selection.
    - ComboBox event handling (ComboSelectAfter, ItemPressedAfter, Matrix column handling).

3. **Form Identifiers**
    - Custom Form UID: `FRMSBGRP`
    - System Form Type Examples:
        - `150` → Item Master Data
        - `9999` → System Choose From List form (used for value transfer).

---

## How It Works

### 1. Filtering Flow:

- **Item Series → Item Group → Item Sub Group**

Example:

| Item Series | Visible Item Groups | Visible Item Sub Groups  |
|-------------|--------------------|--------------------------|
| FG          | Finished Group 1, Finished Group 2 | MLV, Sub-Finished 1       |
| RM          | Raw Material 1, Raw Material 2 | Powder, Granules         |

---

### 2. Update Propagation Logic:

When an Item Sub Group name is updated through Sub Group Master Data Form,  
the changes automatically reflect in all related Item Master Data entries linked to that subgroup.

Example:

- User updates: `Sub Group` → `MLV`
- The change reflects under `Item Master Data` → `Item Sub Group` field.

---

## Installation Notes

- Compatible with SAP Business One UI API SDK version 9.x or higher.
- Ensure `SAPbouiCOM.Framework` is referenced in your project.
- Update connection details in the main add-on initialization file before deployment.

---

## Development Notes

- Language: C# (.NET Framework)
- SDK: SAPbouiCOM, SAPbobsCOM
- Key Files:
    - `SUBGRP_MASTER_DATA_FRM.cs`
    - `ComboLoadAfter.cs`
    - `Program.cs`
- Custom Table: `@FIL_MH_SUBGRP`
- Dependencies:
    - `Global.GFunc.setComboBoxValue()` helper handles combo box value population from SQL queries.

---

## License

Private Custom Development — Intended for internal use by authorized SAP B1 customers.

---

## Author

Asif Iftekher Fahim  
[https://github.com/IftekherFahim27](https://github.com/IftekherFahim27)

