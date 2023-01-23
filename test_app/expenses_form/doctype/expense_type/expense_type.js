// Copyright (c) 2023, test and contributors
// For license information, please see license.txt

frappe.ui.form.on("Expense Type", {
    // in the child table 'Expense Line'after selecting expense_type, if the id of the expense_type is 1496785830 and the type is 'Mileage', then se the 
    // the property of amount to hidden and the property of km to visible

    // expense_type: function(frm) {
    //     frappe.call({
    //         method: "frappe.client.get_value",
    //         args: {
    //             doctype: "Expense Type",
    //             filters: {
    //                 name: frm.doc.expense_type
    //             },
    //             fieldname: ["id", "type"]
    //         },
    //         callback: function(r) {
    //             if (r.message.id == 1496785830 && r.message.type == "Mileage") {
    //                 frm.set_df_property("amount", "hidden", 1);
    //                 frm.set_df_property("km", "hidden", 0);
    //             }
    //         }
    //     });
    // }

});
