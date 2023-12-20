$.validator.addMethod("mustbetrue",
    function (value, element, parameters) {
        return element.checked;
    });

$.validator.unobtrusive.adapters.add("mustbetrue", [], function (options) {
    options.rules.mustbetrue = {};
    options.messages["mustbetrue"] = options.message;
});

jQuery.validator.setDefaults({
    ignore: [],
    highlight: function (element, errorClass, validClass) {
        $(element).closest('.form-group').addClass('form-group--error');
    },
    unhighlight: function (element, errorClass, validClass) {
        $(element).closest('.form-group').removeClass('form-group--error');
    }
});

$(document).ready(function () {
    $('#registrationLink').on('click', function () {
        $('#registerUser').removeClass('d-none');
        $('#linkExisting').addClass('d-none');
    });

    $('#SsoRegisterUserForm_StaffGroupId').on('change', function () {
        updateRoles($(this).val());
        updateGrades(0);
        getProfessionalBody(0);
    });

    $('#SsoRegisterUserForm_JobRoleId').on('change', function () {
        updateGrades($(this).val());
        getProfessionalBody($(this).val());
    });

    //updateRoles($('#SsoRegisterUserForm_StaffGroupId').val());
    //updateGrades($('#SsoRegisterUserForm_JobRoleId').val());
    getProfessionalBody($('#SsoRegisterUserForm_JobRoleId').val());

    $("#SsoRegisterUserForm_MedicalCouncilNumber").rules("add", {
        required: function (element) {
            return $('#professionalBody_container').is(':visible');
        },
        messages: {
            required: "Enter your medical council number"
        }
    });

    var locations = new Bloodhound({
        datumTokenizer: Bloodhound.tokenizers.obj.whitespace('value'),
        queryTokenizer: Bloodhound.tokenizers.whitespace,
        remote: {
            url: '/api/RegistrationInfo/LocationBySearchCriteria/%QUERY',
            wildcard: '%QUERY'
        }
    });

    $('#SsoRegisterUserForm_Location').typeahead(null, {
        minLength: 2,
        limit: 9999,
        source: locations,
        display: function (data) {
            return data.name + ', ' + data.address;
        },
        templates: {
            suggestion: function (data) {
                return '<p>' + data.name + '<br/>' + data.address + '</p>';
            }
        }
    }).bind('typeahead:select', function (ev, suggestion) {
        $('#SsoRegisterUserForm_LocationId').val(suggestion.id);
    });
});

function updateRoles(staffGroupId) {
    var roles = $('#SsoRegisterUserForm_JobRoleId');
    $(roles).empty().css('background-color', '#f5edad');

    if (staffGroupId == 0) {
        $(roles).append('<option value="0">Please choose...</option>').css('background-color', '#fff');

    }
    else {

        $.ajax({
            type: 'GET',
            data: 'staffGroupId=' + staffGroupId,
            url: "/api/RegistrationInfo/GetJobRoles",
            dataType: "json",
            async: false,
            success: function (data) {
                var html = '<option value="0">Please choose..</option>';
                var len = data.length;
                for (var i = 0; i < len; i++) {
                    html += '<option value="' + data[i].id + '">' + data[i].name + '</option>';
                }
                $(roles).append(html).css('background-color', '#fff');

            },
            error: function (data) {
                $(roles).css('background-color', '#f55');
                alert('error:' + data);
            }
        });
    }
}

function updateGrades(jobRoleId) {
    var $grades = $('#SsoRegisterUserForm_GradeId');
    $('#grade_container').removeClass('d-none');
    $grades.empty().append('<option value="0">Please choose...</option>').css('background-color', '#fff');

    if (jobRoleId > 0) {
        $.ajax({
            type: 'GET',
            data: 'jobRoleId=' + jobRoleId,
            url: "/api/RegistrationInfo/GetGrades",
            dataType: "json",
            async: false,
            success: function (data) {
                if (Array.isArray(data)) {
                    for (var i = 0; i < data.length; i++) {
                        $grades.append('<option value="' + data[i].id + '">' + data[i].name + '</option>');
                    }
                    if (data.length == 1) {
                        $grades.val(data[0].id);
                    }
                }
            },
            error: function (data) {
                $grades.empty().css('background-color', '#f5edad');
                alert('error:' + data);
            }
        });
    }
}

function getProfessionalBody(jobRoleId) {
    if (jobRoleId == 0) {
        $("#professionalBody_container").addClass('d-none');
        $("#SsoRegisterUserForm_MedicalCouncilNumber").val('');
    }
    else {
        $.ajax({
            type: 'GET',
            data: 'jobRoleId=' + jobRoleId,
            url: "/api/RegistrationInfo/GetMedicalCouncil",
            dataType: "json",
            async: false,
            success: function (data) {
                if (data.id && data.id != 0) {
                    $("#professionalBody_container").removeClass('d-none');
                } else {
                    $("#professionalBody_container").addClass('d-none');
                    $("#SsoRegisterUserForm_MedicalCouncilNumber").val('');
                }
            },
            error: function (data) {
                $("#professionalBody_container").addClass('d-none');
                $("#SsoRegisterUserForm_MedicalCouncilNumber").val('');
                alert('error:' + data);
            }
        });
    }
}
