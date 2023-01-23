// Copyright (c) 2023, test and contributors
// For license information, please see license.txt

frappe.ui.form.on("Expense Claim", {
    
    onload_post_render: function(frm) {
        frm.call("get_employee")
            .then(r => {
                if(r.message) {
                    let employee = r.message;
                    console.log(employee);
                    frm.set_query("employee_id", function() {
                        return {
                            filters: [
                                ["Employee", "name", "in", employee]
                            ]
                        };
                    });
                }
            });
    },
    // filter the employee_id field based on the user logged in by only showing the records that have the same email

    refresh: function(frm) {
        // filter the employee_id field based on the user logged in by only showing the records that have the same email
        frm.set_query("employee_id", function() {
            return {
                filters: {
                    //filter by matching the email to the email of the user in the current session
                    email: frappe.session.user.email
                }
            };
        });
    },


	refresh(frm){
        // change the text of the button "add-row" to "Add Expense"
        frm.fields_dict.expense_line.grid.grid_buttons.find(".grid-add-row").html("Add Expense");
    },
    // after the user selects an employee_id, fill the employee_name field
    
    employee_id: function(frm) {
        // get the employee name from the employee_id useing the get_value method
        frappe.call({

            method: "frappe.client.get_value",
            args: {
                doctype: "Employee",
                filters: {
                    name: frm.doc.employee_id
                },
                fieldname: "full_name"
            },

            callback: function(r) {
                // set the employee_name field to the value returned from the server
                frm.set_value("full_name", r.message.full_name);
            }
        });

        // based on the employee id grab the department and set the value of the department field
        frappe.call({
            method: "frappe.client.get_value",
            args: {
                doctype: "Employee",
                filters: {
                    name: frm.doc.employee_id
                },
                fieldname: "department_name"
            },
            callback: function(r) {
                frm.set_value("department", r.message.department_name);
            }
        });
    },

    validate: function(frm) {
        // validate the total amount of the claim
        var total = 0;
        frm.doc.expense_line.forEach(function(d) {
            total += d.amount;
        });
        frm.set_value("total_amount_entered", total);
    }
});


frappe.ui.form.on("Expense Line 1",  {

    expense_type: function(frm, cdt, cdn) {
        var expense_type = frappe.meta.get_docfield("Expense Line 1", "expense_type", cdn);
        var row = locals[cdt][cdn];
        expense_type.read_only = 1;
        
        var amt = frappe.meta.get_docfield("Expense Line 1", "amount", cdn);
        var km = frappe.meta.get_docfield("Expense Line 1", "km", cdn);
        frappe.call({
            method: "frappe.client.get_value",
            args: {
                doctype: "Expense Type",
                filters: {
                    name: row.expense_type
                },
                fieldname: ["type"]
            },
            callback: function(r) {
                if (r.message.type == "Mileage") {
                    km.read_only = 0;
                    //set the value of km to 0
                    frappe.model.set_value(cdt, cdn, "km", 0);
                    
                } else {
                    amt.read_only = 0;
                    //set the value of amount to 0
                    frappe.model.set_value(cdt, cdn, "amount", 0);
                }
                cur_frm.fields_dict["expense_line_1"].grid.grid_rows[cdn].refresh();
                cur.frm.refresh_field("expense_line_1");
            }
        });

        // access the doctype "Expense Type" and get the field "id" and access the doctype "Department" and get
        // the field "department_number" then set the value of the field "account_distribution" in the child table 
        // using the convination "id-department_number"
        frappe.call({
            method: "frappe.client.get_value",
            args: {
                doctype: "Expense Type",
                filters: {
                    name: row.expense_type
                },
                fieldname: ["id"]
            },
            callback: function(r) {
                frappe.call({
                    method: "frappe.client.get_value",
                    args: {
                        doctype: "Employee",
                        filters: {
                            name: frm.doc.employee_id
                        },
                        fieldname: ["department_id"]
                    },
                    callback: function(r1) {
                        frappe.model.set_value(cdt, cdn, "account_distribution", r.message.id + "-" + r1.message.department_id);
                    }
                });
            }
        });

    },

    km: function(frm, cdt, cdn) {
        var row = locals[cdt][cdn];
        // get the rate from the single doc type "Gas Rate"
        
        frappe.call({
            method: "frappe.client.get_value",
            args: {
                doctype: "Gas Rate",
                filters: {
                    name: "Gas Rate"
                },
                fieldname: ["rate"]
            },
            callback: function(r) {
                //set the value of amount to the rate * km
                frappe.model.set_value(cdt, cdn, "amount", r.message.rate * row.km);
                frappe.model.set_value(cdt, cdn, "gas_rate", r.message.rate);
            }
        });
    },
});