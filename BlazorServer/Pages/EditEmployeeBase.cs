using BlazorServer.Services;
using EmployeeManagement.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorServer.Pages
{
    public class EditEmployeeBase : ComponentBase
    {
        public int id { get; set; }

        [Inject]
        public IEmployeeService EmployeeService { get; set; }

        public Employee Employee { get; set; } = new Employee();

        [Parameter]
        public string Id { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Employee = await EmployeeService.GetEmployee(int.Parse(Id));
        }
    }
}
