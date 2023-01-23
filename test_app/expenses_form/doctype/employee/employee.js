// Copyright (c) 2023, test and contributors
// For license information, please see license.txt

 frappe.ui.form.on("Employee", {
	

    //fill the full_name field on save
    validate: function(frm) {
        frm.set_value("full_name", frm.doc.first_name + " " + frm.doc.last_name);
    }
});
