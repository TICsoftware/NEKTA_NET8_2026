let debounceTimer;
let pageNABL = 1;
let pagenoNNABL = 1;
const pagesize = 10;

// 🔹 COMMON FILTER BUILDER
function getFilters() {
    return {
        Specimen_Id: getValue("#ddSpecimen"),
        TypeTestId: getValue("#ddTypeTest"),
        OrganId: getValue("#ddOrgan"),
        DepartmentId: getValue("#ddDepartment"),
        Title: $(".testdirectory-search").val() || null
    };
}

// 🔹 VALUE HELPER
function getValue(selector) {
    const val = $(selector).val();
    return (!val || val === "0") ? null : val;
}

// 🔹 CARD BUILDER (single source)
function buildCard(item) {
    return `
    <div class="bg-white rounded-2xl drop-shadow-custom p-5 testdirectory-card-wrappper dark-bglight">
        <h3 class="text-20 font-semibold mb-4">${item.title || ''}</h3>
        <div class="grid grid-cols-1 sm:grid-cols-2 gap-y-4 gap-x-4">
            <div class="no-margin-wrap"><p class="text-primary">Specimen</p><p>${item.specimen || '-'}</p></div>
            <div class="no-margin-wrap"><p class="text-primary">Type of test</p><p>${item.typeTest || '-'}</p></div>
            <div class="no-margin-wrap"><p class="text-primary">Organ</p><p>${item.organ || '-'}</p></div>
            <div class="no-margin-wrap"><p class="text-primary">Department/ Section</p><p>${item.department || '-'}</p></div>
        </div>
    </div>`;
}

// 🔹 RENDER FUNCTION (reusable)
function renderData(tests) {
    let nablHtml = "";
    let nonNablHtml = "";

    tests.forEach(item => {
        const card = buildCard(item);
        if (item.covered_under_NABL == 1) nablHtml += card;
        else if (item.covered_under_NABL == 2) nonNablHtml += card;
    });

    $("#DivNABL").html(nablHtml || "<p>No data found</p>");
    $("#DivNONABL").html(nonNablHtml || "");
}
// 🔹 LOAD MAIN DATA
function loadTests() {

    $("#DivNABL, #DivNONABL").html('<p>Loading...</p>');

    $.post('/TestDirectory/LoadTests', { detail: getFilters() })
    .done(res => {

        // 🔥 ALWAYS update counts FIRST
        $("#nabl1Count").text(`(${res?.nabl1Count || 0})`);
        $("#nabl2Count").text(`(${res?.nabl2Count || 0})`);

        // 🔹 No data case
        if (!res?.tests || res.tests.length === 0) {
            $("#DivNABL").html('<p>No data found</p>');
            $("#DivNONABL").html('');
            toggleLoadButtons(0, 0);
            return;
        }

        // 🔹 Render data
        renderData(res.tests);

        // 🔹 Load more buttons
        toggleLoadButtons(res.nabl1Count, res.nabl2Count);

    })
    .fail(() => {
        $("#DivNABL").html('<p>Error loading data</p>');
        $("#DivNONABL").html('');

        // 🔥 Reset counts on error
        $("#nabl1Count").text("(0)");
        $("#nabl2Count").text("(0)");

        toggleLoadButtons(0, 0);
    });
}

// 🔹 LOAD MORE

// function loadMore(nablOption) {

//     const pageno = nablOption === 1 ? pageNABL : pagenoNNABL;

//     $.ajax({
//         url: '/TestDirectory/LoadMoreTests',
//         type: 'POST',
//         contentType: 'application/json',
//         data: JSON.stringify({
//             detail: getFilters(),
//             pageno: pageno,
//             pagesize: pagesize,
//             nablOption: nablOption
//         }),
//         success: function (res) {

//             if (!res.tests?.length) {
//                              toggleSingleButton(nablOption, false);
//                              return;
//                         }
            
//                          let html = res.tests.map(buildCard).join("");
                
//                          if (nablOption === 1) $("#DivNABL").append(html);
//                          else $("#DivNONABL").append(html);
                
//                          if ((pageno * pagesize) >= res.total) {
//                              toggleSingleButton(nablOption, false);
//                          }
//         },
//         error: function (err) {
//             console.error("LoadMore Error:", err.responseText);
//         }
//     });
// }

 function loadMore(nabloption) {

     const pageno = nabloption === 1 ? pageNABL : pagenoNNABL;

     $.post('/TestDirectory/LoadMoreTests', {
         detail: getFilters(),
         pageno,
         pagesize,
         nabloption
     })
     .done(res => {
         
         if (!res.tests?.length) {
             toggleSingleButton(nabloption, false);
             return;
         }

         let html = res.tests.map(buildCard).join("");

         if (nabloption === 1) $("#DivNABL").append(html);
         else $("#DivNONABL").append(html);

         if ((pageno * pagesize) >= res.total) {
            toggleSingleButton(nabloption, false);
        }
     })
     .fail((xhr, status, error) => {

        console.error("LoadMore Error:", error);
        console.error("Status:", status);
        console.error("Response:", xhr.responseText);

        let message = "Something went wrong. Please try again.";

        // Optional: show server message if available
        if (xhr.responseJSON?.message) {
            message = xhr.responseJSON.message;
        }

        //alert(message);

        // Optional: disable button to prevent spam
        toggleSingleButton(nabloption, false);
    })
    .always(() => {
        // Optional: stop loader/spinner here
        console.log("Request completed");
    });
 }

// 🔹 BUTTON HANDLING
function toggleLoadButtons(nablCount, nonNablCount) {
    toggleSingleButton(1, nablCount > pagesize);
    toggleSingleButton(2, nonNablCount > pagesize);
}

function toggleSingleButton(nabloption, show) {
    const btn = nabloption === 1 ? "#btnLoadMoreNABL" : "#btnLoadMoreNONNABL";
    $(btn).toggle(show);
}

// 🔹 CLEAR FILTERS
$("#btnClearFilters").click(function () {

    $(".fy-select").val('');
    $(".testdirectory-search").val('');

    $(".fy-select").each(function () {
        const txt = $(this).find("option:first").text();
        $(this).siblings(".selected-value").find("span").text(txt);
    });

    pageNABL = 1;
    pagenoNNABL = 1;

    toggleClearButton();
    loadTests();
});

// 🔹 FILTER CHANGE
$("#ddSpecimen, #ddTypeTest, #ddOrgan, #ddDepartment")
.change(() => { toggleClearButton(); resetAndReload(); });

// 🔹 SEARCH (debounced)
$(".testdirectory-search").on("keyup", function () {
    clearTimeout(debounceTimer);

    debounceTimer = setTimeout(() => {
        toggleClearButton();
        resetAndReload();   // 🔥 IMPORTANT
    }, 400);
});

// 🔹 LOAD MORE CLICK
$("#btnLoadMoreNABL").click(() => { pageNABL++; loadMore(1); });
$("#btnLoadMoreNONNABL").click(() => { pagenoNNABL++; loadMore(2); });

// 🔹 CLEAR BUTTON VISIBILITY
function hasActiveFilters() {
    return Object.values(getFilters()).some(v => v !== null && v !== "");
}

function toggleClearButton() {
    $("#btnClearFilters").toggle(hasActiveFilters());
}


function resetAndReload() {

    pageNABL = 1;
    pagenoNNABL = 1;

    $("#DivNABL").html('');
    $("#DivNONABL").html('');

    $("#btnLoadMoreNABL").show();
    $("#btnLoadMoreNONNABL").show();

    loadTests();   // 🔥 IMPORTANT
}