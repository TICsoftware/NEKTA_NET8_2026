$("#btnSearchRedirect").click(function () {

    
    let filters = {
        Specimen_Id: getValue("#ddSpecimen"),
        TypeTestId: getValue("#ddTypeTest"),
        OrganId: getValue("#ddOrgan"),
        DepartmentId: getValue("#ddDepartment"),
        Title: $("#Title").val()
    };

    $.post('/TestDirectory/StoreFilters', { model: filters })
        .done(function () {
            window.location.href = "/test-directory";
        });
});


function getValue(selector) {
    let val = $(selector).val();
    return (val === "" || val === "0") ? null : val;
}