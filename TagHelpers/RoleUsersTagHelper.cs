﻿using HospitalAppointmentProject1.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace HospitalAppointmentProject1.TagHelpers
{
    [HtmlTargetElement("td", Attributes = "asp-role-users")]
    public class RoleUsersTagHelper : TagHelper
    {
        private readonly RoleManager<AppRole> _roleManager;

        private readonly UserManager<AppUser> _userManager;
        public RoleUsersTagHelper(RoleManager<AppRole> roleManager, UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        [HtmlAttributeName("asp-role-users")]
        public string RoleId { get; set; } = null!;

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var userNames = new List<string>();

            var role = await _roleManager.FindByIdAsync(RoleId);

            if (role != null && role.Name != null)
            {
                foreach (var user in _userManager.Users)
                {
                    if (await _userManager.IsInRoleAsync(user, role.Name))
                    {
                        userNames.Add(user.UserName);
                    }

                }
                output.Content.SetHtmlContent(userNames.Count == 0 ? "No Users" : setHtml(userNames));
            }


        }

        private string setHtml(List<string> userNames)
        {
            var html = "<ul>";

            foreach (var userName in userNames)
            {
                html += $"<li>{userName}</li>";
            }
            html += "</ul>";

            return html;
        }
    }
}
