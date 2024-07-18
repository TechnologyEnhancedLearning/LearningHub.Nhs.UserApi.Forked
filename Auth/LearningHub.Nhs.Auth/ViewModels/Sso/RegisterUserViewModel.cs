namespace LearningHub.Nhs.Auth.ViewModels.Sso
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using elfhHub.Nhs.Models.Common;
    using elfhHub.Nhs.Models.Entities;

    /// <summary>
    /// The single-sign-on register user request.
    /// </summary>
    public class RegisterUserViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterUserViewModel"/> class.
        /// </summary>
        public RegisterUserViewModel()
        {
            this.StaffGroups = new List<StaffGroup>(new[] { new StaffGroup { Id = 0, Name = "Please choose..." } });
            this.JobRoles = new List<JobRoleBasicViewModel>(new[] { new JobRoleBasicViewModel { Id = 0, Name = "Please choose..." } });
            this.Grades = new List<GenericListViewModel>(new[] { new GenericListViewModel { Id = 0, Name = "Please choose..." } });
            this.Specialties = new List<GenericListViewModel>(new[] { new GenericListViewModel { Id = 0, Name = "Please choose..." } });
        }

        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        [Required(ErrorMessage = "Enter your email address")]
        [EmailAddress(ErrorMessage = "Enter valid email address")]
        public string EmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        [Required(ErrorMessage = "Enter your first name")]
        [MinLength(1, ErrorMessage = "Enter valid first name")]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        [Required(ErrorMessage = "Enter your last name")]
        [MinLength(1, ErrorMessage = "Enter valid last name")]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the staff group id.
        /// </summary>
        [Required(ErrorMessage = "Select staff group")]
        [Range(1, int.MaxValue, ErrorMessage = "Select staff group")]
        public int? StaffGroupId { get; set; }

        /// <summary>
        /// Gets or sets the job role id.
        /// </summary>
        [Required(ErrorMessage = "Select current role")]
        [Range(1, int.MaxValue, ErrorMessage = "Select current role")]
        public int? JobRoleId { get; set; }

        /// <summary>
        /// Gets or sets the grade id.
        /// </summary>
        [Required(ErrorMessage = "Select grade")]
        [Range(1, int.MaxValue, ErrorMessage = "Select grade")]
        public int? GradeId { get; set; }

        /// <summary>
        /// Gets or sets the medical council number.
        /// </summary>
        public string MedicalCouncilNumber { get; set; }

        /// <summary>
        ///  Gets or sets the specialty id.
        /// </summary>
        [Required(ErrorMessage = "Select specialty")]
        [Range(1, int.MaxValue, ErrorMessage = "Select specialty")]
        public int? SpecialtyId { get; set; }

        /// <summary>
        /// Gets or sets the location id.
        /// </summary>
        [Required(ErrorMessage = "Select place of work")]
        [Range(1, int.MaxValue, ErrorMessage = "Select place of work")]
        public int? LocationId { get; set; }

        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to display staff group option.
        /// </summary>
        public bool ShowStaffGroupOption { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to display job role option.
        /// </summary>
        public bool ShowJobRoleOption { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to display grading option.
        /// </summary>
        public bool ShowGradingOption { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to display speciality option.
        /// </summary>
        public bool ShowSpecialityOption { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to display location.
        /// </summary>
        public bool ShowLocation { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether consented.
        /// </summary>
        [MustBeTrue(ErrorMessage = "Please accept terms and conditions.")]
        public bool Consented { get; set; }

        /// <summary>
        /// Gets the StaffGroups.
        /// </summary>
        public List<StaffGroup> StaffGroups { get; }

        /// <summary>
        /// Gets the JobRoles.
        /// </summary>
        public List<JobRoleBasicViewModel> JobRoles { get; }

        /// <summary>
        /// Gets the grades.
        /// </summary>
        public List<GenericListViewModel> Grades { get; }

        /// <summary>
        /// Gets the Specialties.
        /// </summary>
        public List<GenericListViewModel> Specialties { get; }

        /// <summary>
        /// Set Sso Create User View Model.
        /// </summary>
        /// <param name="vm">Sso Create User View Model.</param>
        public void SetRegistrationInfo(CreateUserViewModel vm)
        {
            vm.ShowLinkUserForm = false;
            var registerForm = vm.SsoRegisterUserForm;

            registerForm.EmailAddress = this.EmailAddress;
            registerForm.FirstName = this.FirstName;
            registerForm.LastName = this.LastName;
            registerForm.StaffGroupId = this.StaffGroupId;
            registerForm.JobRoleId = this.JobRoleId;
            registerForm.GradeId = this.GradeId;
            registerForm.MedicalCouncilNumber = this.MedicalCouncilNumber;
            registerForm.SpecialtyId = this.SpecialtyId;
            registerForm.LocationId = this.LocationId;
            registerForm.Location = this.Location;
        }
    }
}