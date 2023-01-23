# Copyright (c) 2023, test and contributors
# For license information, please see license.txt
import frappe
from frappe.model.document import Document
from frappe.permissions import allow_everything

class ExpenseClaim(Document):
    
    @frappe.whitelist()
    def get_employee(self):
        user = frappe.db.get_value("User", {"name": frappe.session.user}, "email")
        role = frappe.get_roles(frappe.session.user)
        if('Administrator' in role):
            return frappe.get_list("Employee", pluck="name")
        else:
            return frappe.get_list("Employee", filters={'email':user}, pluck="name")

    