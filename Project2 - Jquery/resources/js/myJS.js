//fancybox plugin
$(document).ready(function() {
    $(".fancybox").fancybox({
        openEffect: 'elastic',
        autoSize: false,
        beforeShow: function() {
            /* Disable right click */
            $.fancybox.wrap.bind("contextmenu", function(e) {
                return false;
            });
        }
    });
});



$(document).ready(function() {

    //About
    myXhr('get', {
        path: '/about/'
    }, '#about').done(function(json) {

        var $h3About = $("<h3>").text(json.title);
        $($h3About).attr("class", "subHead");
        var $pDesc = $("<p>").text(json.description);
        var $pQuote = $("<p>").text(json.quote);
        var $pAuthor = $("<p>").text(json.quoteAuthor);

        $("#aboutBox").append($h3About)
        $("#aboutBox").append($pDesc);
        $("#aboutBox").append($pQuote);
        $("#aboutBox").append($pAuthor);

    });

    //undergraduate
    myXhr('get', {
        path: '/degrees/undergraduate/'
    }, '#ug').done(function(json) {

        var $divSlide = $("<div>", {
            class: "slide",
            id: "undergrad"
        });

        $.each(json.undergraduate, function(i, degree) {

            var $divUGMajor = $("<div>").attr("data-degree", (degree.degreeName));
            $($divUGMajor).attr("onclick", 'getConcentration(this, \"/degrees/undergraduate/degreeName\")');
            $($divUGMajor).attr("class", "UTinyBox fancybox");
            $($divUGMajor).attr("data-fancybox-href", "#UgGradDetails");
            $($divUGMajor).css("cursor", "pointer");
            var $h3UDegree = $("<h3>").text(degree.title);
            var $h6UDegree = $("<h6>").attr("class", "subtitle").text(degree.description);

            $($divUGMajor).append($h3UDegree);
            $($divUGMajor).append($h6UDegree);
            $($divSlide).append($divUGMajor);

        }); //end each

        $("#UGBox").append($divSlide);

    }); //end of myxhr undergraduate


    //graduate
    myXhr('get', {
        path: '/degrees/graduate/'
    }, '#grad').done(function(json) {

        var $divSlide = $("<div>", {
            class: "slide",
            id: "graduate"
        });

        $.each(json.graduate, function(i, degree) {

            if (degree.degreeName != "graduate advanced certificates") {

                var $divGMajor = $("<div>").attr("data-degree", (degree.degreeName));
                $($divGMajor).attr("onclick", 'getConcentration(this, \"/degrees/graduate/degreeName\")');
                $($divGMajor).attr("class", "UTinyBox fancybox");
                $($divGMajor).attr("data-fancybox-href", "#gradDetails");
                $($divGMajor).css("cursor", "pointer");
                var $h3GDegree = $("<h3>").text(degree.title);
                var $h6GDegree = $("<h6>").attr("class", "subtitle").text(degree.description);

                $($divGMajor).append($h3GDegree);
                $($divGMajor).append($h6GDegree);
                $($divSlide).append($divGMajor);
            }
        }); //end each

        $("#GBox").append($divSlide);

    }); //end of myxhr graduate


    //Minors
    myXhr('get', {
        path: '/minors/'
    }, '#min').done(function(json) {


        var $divSlide = $("<div>", {
            class: "slide",
            id: "minor"
        });

        $.each(json.UgMinors, function(i, minors) {

            var $divMinor = $("<div>").attr("data-minor", (minors.name));
            $($divMinor).attr("onclick", 'getCourses(this, \"/minors/UgMinors/name\")');
            $($divMinor).attr("class", "minorBox fancybox");
            $($divMinor).attr("data-fancybox-href", "#minorDetails");

            var $h3Minor1 = $("<h3>").text(minors.title);
            var $h3Minor2 = $("<h6>").attr("class", "subtitle").text(minors.name);

            $($divMinor).append($h3Minor1);
            $($divMinor).append($h3Minor2);

            $($divSlide).append($divMinor);

        }); //end each

        $("#MBox").append($divSlide);

    }); //end of myxhr graduate

    //employment
    myXhr('get', {
        path: '/employment/'
    }, '#emp').done(function(json) {

        var introduction = json.introduction;
        var degreeStatistics = json.degreeStatistics;
        var employers = json.employers;
        var careers = json.careers;
        var ct = json.coopTable;
        var et = json.employmentTable;


        $("#emp.pageTitle").append(introduction.title);

        var $divSlide = $("<div>", {
            class: "slide",
            id: "employment"
        });

        var $H5Title;
        var $H7Desc;
        var $StatsDiv;
        var choice;
        var $CareerNames;

        var $Stats = $("<div>").attr("id", "stats");
        var $EmplDiv = $("<div>").attr("id", "eDiv");
        var $CareerDiv = $("<div>").attr("id", "carDiv");
        var arrColor = ['#00796B', '#303F9F', '#FFC107', '#FF5252'];

        $.each(introduction.content, function(i, item) {

            $EmpDiv = $("<div>").attr("class", "empl");
            $H5Title = $("<h3>").text(item.title);
            $H7Desc = $("<h6>").text(item.description).css({
                "font-size": "1.2em",
                "margin": "0px"
            });

            if (item.title == "Cooperative Education") {
                $($EmpDiv).append($Stats);
            }

            $($EmpDiv).append($H5Title);
            $($EmpDiv).append($H7Desc);
            $($divSlide).append($EmpDiv);
        });

        $("#EBox").append($divSlide);

        $.each(degreeStatistics.statistics, function(i, item) {

            $StatsDiv = $("<div>").attr("class", "cBox").css('background-color', arrColor[i]);
            $H6Value = $("<h6>").text(item.value).css({
                'font-size': "2em",
                "color": "white",
                "margin": "0px"
            });
            $H6Desc = $("<h6>").text(item.description).css({
                'font-size': "1.2em",
                "color": "white",
                "font": "Trebuchet",
                "margin": "1% 0px"
            });

            $($StatsDiv).append($H6Value);
            $($StatsDiv).append($H6Desc);

            $($Stats).append($StatsDiv);
        });

        // code to get Employers
        var $EmplTitle = $("<h5>").text(employers.title);

        $($EmplDiv).append($EmplTitle);

        $.each(employers.employerNames, function(j, t) {
            $EmpNames = $("<h7>").text(t).css("font-size", "1.2em");
            $($EmpNames).attr("class", "singleLine");

            $($EmplDiv).append($EmpNames);
        });
        $($divSlide).append($EmplDiv);



        // code to get career titles
        var $careerTitle = $("<h5>").text(careers.title);


        $($CareerDiv).append($careerTitle);

        $.each(careers.careerNames, function(j, t) {
            $CareerNames = $("<h7>").text(t).css("font-size", "1.2em");
            $($CareerNames).attr("class", "singleLine");

            $($CareerDiv).append($CareerNames);
        });
        $($divSlide).append($CareerDiv);


        //Coop table
        var ctDIV = $('<div>').attr({
            "class": "ctBox fancybox",
            "onclick": "getCTable()",
            "data-fancybox-href": "#dtable"
        });

        var cTitle = $("<h2>").text(ct.title).css({
            "color": "white",
            "text-align": "center",
            "vertical-align": "middle",
            "line-height": "200px"
        });
        $(ctDIV).append(cTitle);
        $("#coopBox").append(ctDIV);

        //employment table
        var etDIV = $('<div>').attr({
            "class": "etBox fancybox",
            "onclick": "getETable()",
            "data-fancybox-href": "#Etable"
        });

        var eTitle = $("<h2>").text(et.title).css({
            "color": "white",
            "text-align": "center",
            "vertical-align": "middle",
            "line-height": "200px"
        });

        $(etDIV).append(eTitle);
        $("#emplBox").append(etDIV);

    }); //end of myxhr employment


    //People
    myXhr('get', {
        path: '/people/'
    }, '#ppl').done(function(json) {

        //faculty
        var facultyDIV = $('<div>').attr({
            "class": "ctBox fancybox",
            "onclick": "getFacTable()",
            "data-fancybox-href": "#facultyDetails"
        });

        var cTitle = $("<h2>").text("Faculty").css({
            "color": "white",
            "text-align": "center",
            "vertical-align": "middle",
            "line-height": "200px"
        });

        $(facultyDIV).append(cTitle);
        $("#pplBox").append(facultyDIV);

        //staff
        var staffDIV = $('<div>').attr({
            "class": "etBox fancybox",
            "onclick": "getStaffTable()",
            "data-fancybox-href": "#facultyDetails"
        });

        var cTitle = $("<h2>").text("Staff").css({
            "color": "white",
            "text-align": "center",
            "vertical-align": "middle",
            "line-height": "200px"
        });

        $(staffDIV).append(cTitle);
        $("#pplBox").append(staffDIV);

    }); //end myxhr People

    //research by interestArea
    myXhr('get', {
        path: '/research/byInterestArea/'
    }, '#ra').done(function(json) {

        var topics = json.byInterestArea;

        $.each(topics, function(i, item) {

            var h3Tag = $("<h3>").text(item.areaName);
            $("#accordion").append(h3Tag);

            var accordDiv = $("<div>");
            $("#accordion").append(accordDiv);

            var ulList = $("<ul>").attr("id", "list");
            $(accordDiv).append(ulList);

            $.each(item.citations, function(j, cit) {
                //console.log(cit);
                var listItem = $("<li>").text(cit);
                $(ulList).append(listItem);
            }); //end for loopBottom

        }); //end for loopTop

        $(function() {
            $("#accordion").accordion({
                heightStyle: "content",
                active: false,
                collapsible: true
            });
            $(".ui-accordion [role=tab]").unbind('keydown');
        });

    }); //end of myXhr research by interestArea

    //reserach ByFaculty
    myXhr('get', {
        path: '/research/byFaculty/'
    }, '#rbf').done(function(json) {

        //console.log(json);
        var rByFac = json.byFaculty;
        //console.log(rByFac);

        $.each(rByFac, function(i, item) {


            var imgSrc = "https://ist.rit.edu/assets/img/people/" + item.username + ".jpg";

            //console.log(imgSrc);
            var aTag = $("<a>").attr({
                "class": "fancybox",
                "rel": "gallery1",
                "data-fancybox-href": "#rbfDetails",
                "onclick": "getFacResearch('" + item.username + "')"
            });

            var imgTag = $("<img>").attr({
                "class": "facImg",
                "src": imgSrc,
                "alt": "Faculty Images",
                "height": "120px",
                "width": "120px"
            });

            $(aTag).html(imgTag);
            $("#rbfBox").append(aTag);
        });

    }); //end myXHR by Faculty

    //Student Resources
    myXhr('get', {
        path: '/resources/'
    }, '#sr1').done(function(json) {

        //display heading on the page
        $.each(json, function(i, heading) {
            // console.log(heading);
            if (i != "title" && i != "subTitle") {
                var headDiv = $("<div>").attr({
                    "class": "srDiv fancybox",
                    "data-fancybox-href": "#srDetails",
                    "name": heading,
                    "onclick": "getData('" + i + "')"
                });
                console.log(i);
            } //end if

            //insert spaces before capital letter
            var s = i.replace(/([A-Z])/g, ' $1').trim();
            var headH3 = $("<h3>").text(s);
            $(headDiv).append(headH3);
            $("#srBox").append(headDiv);

        });

    }); //end myXHR Student resources

    //footer
    myXhr('get', {
        path: '/footer/'
    }, '#sr1').done(function(json) {

        var socialLink = json.social;

        $("#links").append($("<h3>").text(socialLink.title));
        $("#links").append($('<a></a>').attr("href", socialLink.tweet).text("Tweet"));
        $("#links").append($("<p>").text(socialLink.by));
        $("#links").append($('<a></a>').attr("href", socialLink.twitter).text("Twitter"));
        $("#links").append($('<a></a>').attr("href", socialLink.facebook).text("Facebook"));

        var quickLink = json.quickLinks;

        $.each(quickLink, function(i, item) {
            var alinkTag = $('<a></a>').attr("href", item.href).text(item.title);
            $("#qlinks").append(alinkTag)
        });

        var cRite = json.copyright;

        $("#copyrite").append($("<h7>").text(cRite.title));
        $("#copyrite").append($("<h7>").html(cRite.html));

        $("#news").append($('<a></a>').attr("href", json.news).text("News"));

    }); //end myXhr footer

}); //end document ready function

function getData(head) {
    myXhr('get', {
        path: '/resources/'
    }, "#srDetails").done(function(json) {

        $("#srDetails").empty();

        var newJSON = json[head];

        //if study abroad
        if (head == "studyAbroad") {
            var h3Title = $("<h3>").text(newJSON.title);
            var h6desc = $("<h6>").text(newJSON.description);
            var placesDiv = $("<div>").attr("class", "place");

            $.each(json[head].places, function(i, item) {

                H5Title = $("<h3>").text(item.nameOfPlace);
                H7Desc = $("<h6>").attr("class", "subtitle").text(item.description);

                $(placesDiv).append(H5Title);
                $(placesDiv).append(H7Desc);
            });

            $("#srDetails").append(placesDiv);
            //
        } else if (head == "studentServices") {

            var advisingDIV = $("<div>");
            var h3Title = $("<h3>").text(newJSON.title);
            $(advisingDIV).append(h3Title);

            //Academic Advisors
            var h4Title = $("<h4>").text(newJSON.academicAdvisors.title);
            var H7Desc = $("<h6>").attr("class", "subtitle").text(newJSON.academicAdvisors.description);
            $(advisingDIV).append(h4Title);
            $(advisingDIV).append(H7Desc);

            var h4Title = $("<h4>").text(newJSON.academicAdvisors.faq.title);
            var h4Link = $("<h4>").text(newJSON.academicAdvisors.faq.contentHref);
            $(advisingDIV).append(h4Title);
            $(advisingDIV).append(h4Link);
            console.log(newJSON.professonalAdvisors.title);
            //Professional academicAdvisors
            var h4Title = $("<h4>").text(newJSON.professonalAdvisors.title);

            $.each(newJSON.professonalAdvisors.advisorInformation, function(i, item) {
                var h5Name = $("<h5>").text(item.name);
                var h6Dept = $("<p>").text(item.department);
                var h6Email = $("<p>").text(item.email);
                $(advisingDIV).append(h5Name);
                $(advisingDIV).append(h6Dept);
                $(advisingDIV).append(h6Email);
            });

            //Faculty Advisors
            var h4Title = $("<h4>").text(newJSON.facultyAdvisors.title);
            var H7Desc = $("<h6>").attr("class", "subtitle").text(newJSON.facultyAdvisors.description);
            $(advisingDIV).append(h4Title);
            $(advisingDIV).append(H7Desc);

            //IST minor advising
            var h4Title = $("<h4>").text(newJSON.istMinorAdvising.title);
            $(advisingDIV).append(h4Title);

            $.each(newJSON.istMinorAdvising.minorAdvisorInformation, function(i, item) {
                var h5Name = $("<h5>").text(item.title);
                var h6Dept = $("<p>").text(item.advisor);
                var h6Email = $("<p>").text(item.email);

                $(advisingDIV).append(h5Name);
                $(advisingDIV).append(h6Dept);
                $(advisingDIV).append(h6Email);
            });


            $("#srDetails").append(advisingDIV);
        } else if (head == "tutorsAndLabInformation") {
            //Tutors and Lab
            var tutorLabDiv = $("<div>");
            console.log(newJSON.title);
            var h4Title = $("<h4>").text(newJSON.title);
            var H7Desc = $("<h6>").attr("class", "subtitle").text(newJSON.description);
            $(tutorLabDiv).append(h4Title);
            $(tutorLabDiv).append(H7Desc);

            $("#srDetails").append(tutorLabDiv);
        }
        //end tutors and Lab
        else if (head == "studentAmbassadors") {
            //student ambassadors
            console.log("inside");
            var ambDiv = $("<div>");
            var h4Title = $("<h4>").text(newJSON.title);
            var H7Desc = $("<h6>").attr("class", "subtitle").text(newJSON.ambassadorsImageSource);
            $(ambDiv).append(h4Title);
            $(ambDiv).append(H7Desc);

            $.each(newJSON.subSectionContent, function(i, item) {
                var h4Title = $("<h4>").text(item.title);
                var H7Desc = $("<h6>").attr("class", "subtitle").text(item.description);
                $(ambDiv).append(h4Title);
                $(ambDiv).append(H7Desc);

            });

            var h4Title = $("<h4>").text(newJSON.applicationFormLink);
            var H7Desc = $("<h6>").attr("class", "subtitle").text(newJSON.note);
            $(ambDiv).append(h4Title);
            $(ambDiv).append(H7Desc);

            $("#srDetails").append(ambDiv);
        } //end student ambassadors
        else if (head == "coopEnrollment") {
            var copDiv = $("<div>");

            var h4Title = $("<h4>").text(newJSON.title);
            $(copDiv).append(h4Title);

            $.each(newJSON.enrollmentInformationContent, function(i, item) {
                var h4Title = $("<h4>").text(item.title);
                var H7Desc = $("<h6>").attr("class", "subtitle").text(item.description);
                $(copDiv).append(h4Title);
                $(copDiv).append(H7Desc);
            });

            var H7Desc = $("<h6>").attr("class", "subtitle").text(newJSON.RITJobZoneGuidelink);
            $(copDiv).append(H7Desc);

            $("#srDetails").append(copDiv);
        } //end coop
        else if (head == "forms") {
            var formsDiv = $("<div>");

            $.each(newJSON.graduateForms, function(i, item) {
                var h4Title = $("<h4>").text(item.formName);
                var H7Desc = $("<h6>").attr("class", "subtitle").text(item.href);
                $(formsDiv).append(h4Title);
                $(formsDiv).append(H7Desc);
            });

            $.each(newJSON.undergraduateForms, function(i, item) {
                var h4Title = $("<h4>").text(item.formName);
                var H7Desc = $("<h6>").attr("class", "subtitle").text(item.href);
                $(formsDiv).append(h4Title);
                $(formsDiv).append(H7Desc);
            });
            $("#srDetails").append(formsDiv);
        } //end forms

    }); //end of myXhr function

} //end getData function

function getFacResearch(id) {

    $("#rbfDetails").empty();

    myXhr('get', {
        path: '/research/byFaculty/'
    }, '#rbf').done(function(json) {

        var facRes = json.byFaculty;

        var result = $.grep(facRes, function(e) {
            return e.username == id;
        });

        $("#rbfDetails").append($("<h3>").attr("class", "inFancyHeading").text(result[0].facultyName));
        var ulTag = $("<ul>").attr("id", "citationList");

        $.each(result[0].citations, function(i, item) {
            var liTag = $("<li>").attr("class", "liItems").text(item);
            $(ulTag).append(liTag);
        });

        $("#rbfDetails").append(ulTag);
    }); //end myXHR getFacResearch

} //end getFacResearch

function getStaffTable() {

    $("#facultyDetails").empty();

    myXhr('get', {
        path: '/people/'
    }, '#ppl').done(function(json) {

        var staffDetails = json.staff;

        console.log(staffDetails);

        var arr
        var staffDetails;

        $.each(staffDetails, function(i, item) {

            if (i == 0) {
                arr = (Object.keys(item));
            }
            var fachead = $("<h2>").attr("class", "facheader");
            $(fachead).text(item.name);
            console.log(i);
            var facPic = $("<img>").attr("src", item.imagePath);

            // var facdetails = $("<p>").text(item.title + "\n" + item.interestArea + "\n" + item.office + "\n" + item.website + "\n" + item.phone + "\n" + item.email + "\n" + item.twitter + "\n" + item.facebook);

            var facTitle = $("<p>").text(arr[2] + ": " + item.tagline);
            var facTitle = $("<p>").text(arr[4] + ": " + item.title);
            var facInterestArea = $("<p>").text(arr[5] + ": " + item.interestArea);
            var facOffice = $("<p>").text(arr[6] + ": " + item.office);
            var facWebsite = $("<p>").text(arr[7] + ": " + item.website);
            var facPhone = $("<p>").text(arr[8] + ": " + item.phone);
            var facEmail = $("<p>").text(arr[9] + ": " + item.email);
            var facTwitter = $("<p>").text(arr[10] + ": " + item.twitter);
            var facFacebook = $("<p>").text(arr[11] + ": " + item.facebook);



            $("#facultyDetails").append(fachead);
            $("#facultyDetails").append(facPic);
            $("#facultyDetails").append(facTitle);

            $("#facultyDetails").append(facInterestArea);
            $("#facultyDetails").append(facOffice);

            $("#facultyDetails").append(facWebsite);
            $("#facultyDetails").append(facPhone);
            $("#facultyDetails").append(facEmail);
            $("#facultyDetails").append(facTwitter);
            $("#facultyDetails").append(facFacebook);

        });
    });


} //end getStaffTable function


function getFacTable() {
    myXhr('get', {
        path: '/people/'
    }, '#ppl').done(function(json) {
        var facDetails = json.faculty;

        $("#facultyDetails").empty();
        console.log(facDetails);

        var arr
        var facdetails;

        $.each(facDetails, function(i, item) {

            if (i == 0) {
                arr = (Object.keys(item));
            }
            var fachead = $("<h2>").attr("class", "facheader");
            $(fachead).text(item.name);
            console.log(i);
            var facPic = $("<img>").attr("src", item.imagePath);

            // var facdetails = $("<p>").text(item.title + "\n" + item.interestArea + "\n" + item.office + "\n" + item.website + "\n" + item.phone + "\n" + item.email + "\n" + item.twitter + "\n" + item.facebook);

            var facTitle = $("<p>").text(arr[2] + ": " + item.tagline);
            var facTitle = $("<p>").text(arr[4] + ": " + item.title);
            var facInterestArea = $("<p>").text(arr[5] + ": " + item.interestArea);
            var facOffice = $("<p>").text(arr[6] + ": " + item.office);
            var facWebsite = $("<p>").text(arr[7] + ": " + item.website);
            var facPhone = $("<p>").text(arr[8] + ": " + item.phone);
            var facEmail = $("<p>").text(arr[9] + ": " + item.email);
            var facTwitter = $("<p>").text(arr[10] + ": " + item.twitter);
            var facFacebook = $("<p>").text(arr[11] + ": " + item.facebook);



            $("#facultyDetails").append(fachead);
            $("#facultyDetails").append(facPic);
            $("#facultyDetails").append(facTitle);

            $("#facultyDetails").append(facInterestArea);
            $("#facultyDetails").append(facOffice);

            $("#facultyDetails").append(facWebsite);
            $("#facultyDetails").append(facPhone);
            $("#facultyDetails").append(facEmail);
            $("#facultyDetails").append(facTwitter);
            $("#facultyDetails").append(facFacebook);

        });
    });

} //end of getPplTable

function getCTable() {

    $("#thead").empty();
    $("#tbody").empty();
    myXhr('get', {
        path: "/employment/coopTable/coopInformation"
    }, "#dtable").done(function(json) {

        var ctr = 0;
        $.each(json.coopInformation, function(i, item) {
            ctr = ctr + 1;
            if (i == 0) {
                var arr = (Object.keys(item));
                console.log(arr);
                var tr = $("<tr>");
                var th;
                $.each(arr, function(j, head) {
                    $(tr).append($("<th>").text(head));

                });

                $("#thead").append(tr);
            } //end if

            var col1 = $("<td>").text(item.employer);
            var col2 = $("<td>").text(item.degree);
            var col3 = $("<td>").text(item.city);
            var col4 = $("<td>").text(item.term);

            var tRow = $("<tr>");
            $(tRow).append(col1);
            $(tRow).append(col2);
            $(tRow).append(col3);
            $(tRow).append(col4);

            $("#tbody").append(tRow);
        });
        console.log("Ctr: " + ctr)

        $(document).ready(function() {
            $("#dtable").dataTable({
                "aoColumnDefs": [{
                    "sWidth": "25%",
                    "aTargets": [0]
                }],
                "sPaginationType": "full_numbers"
            });
        });
    }); //end myxhr function

} //end of getTable

/*
 *  function to get data for employment table
 */
function getETable() {

    $("#Ehead").empty();
    $("#Ebody").empty();
    myXhr('get', {
        path: "/employment/employmentTable/professionalEmploymentInformation"
    }, "#Etable").done(function(json) {

        var ctr = 0;
        $.each(json.professionalEmploymentInformation, function(i, item) {
            ctr = ctr + 1;
            if (i == 0) {
                var arr = (Object.keys(item));
                console.log(arr);
                var tr = $("<tr>");
                var th;
                $.each(arr, function(j, head) {
                    $(tr).append($("<th>").text(head));

                });

                $("#Ehead").append(tr);
            } //end if

            var col1 = $("<td>").text(item.employer);
            var col2 = $("<td>").text(item.degree);
            var col3 = $("<td>").text(item.city);
            var col4 = $("<td>").text(item.title);
            var col5 = $("<td>").text(item.startDate);

            var tRow = $("<tr>");
            $(tRow).append(col1);
            $(tRow).append(col2);
            $(tRow).append(col3);
            $(tRow).append(col4);
            $(tRow).append(col5);

            $("#Ebody").append(tRow);
        });
        console.log("Ctr: " + ctr)

        $(document).ready(function() {
            $("#Etable").dataTable({
                "aoColumnDefs": [{
                    "sWidth": "25%",
                    "aTargets": [0]
                }],
                "sPaginationType": "full_numbers"
            });
        });
    }); //end myxhr function

} //end of getETable


function getCourses(dom, fpath) {
    myXhr('get', {
        path: fpath + '=' + $(dom).attr('data-minor')
    }, null).done(function(json) {

        $("#minorDetails").empty();

        var $h3Minor1 = $("<h3>").text(json.title).css({
            'color': 'red',
            'font-size': '1.5em'
        });
        var $h3Minor2 = $("<h3>").text(json.name);
        var $h6Minor = $("<h6>").text(json.description);

        $("#minorDetails").append($h3Minor1);
        $("#minorDetails").append($h3Minor2);
        $("#minorDetails").append($h6Minor);

        $.each(json.courses, function(i, minors) {
            $("#minorDetails").append("<h5>" + minors + "</h5>");

        }); //end each
        if (json.note != "") {
            $("#minorDetails").append(json.note);
        }

    }); //end myXHR

} //end getCourses

/*
 * getConcentration reads the concentration for the degree chosen by the user
 */
function getConcentration(dom, fpath) {
    myXhr('get', {
        path: fpath + '=' + $(dom).attr('data-degree')
    }, null).done(function(json) {

        console.log(json);

        $("#UgGradDetails").empty();
        $("#gradDetails").empty();

        var $h2Conc = $('<h2>').attr("class", "inFancyHeading").text(json.title);
        var $h4Conc = $('<h4>').text("Concentrations");

        if (fpath == "/degrees/undergraduate/degreeName") {
            var dType = "#UgGradDetails";
        } else {
            var dType = "#gradDetails";
        }

        $(dType).append($h2Conc);
        $(dType).append($h4Conc);

        $.each(json.concentrations, function(i, item) {
            var H5ConcItem = $("<h5>").attr("class", "liItems").text(item);
            $(dType).append(H5ConcItem);
            //$(dType).append("<h5>" + item + "</h5>");
        });
    });
} //end function getConcentration

function myXhr(t, d, id) {
    var promise = $.Deferred();
    $.ajax({
        type: t,
        url: 'proxy.php',
        dataType: 'json',
        data: d,
        cache: false,
        async: true,
        beforeSend: function() {
            $(id).append('<img src="resources/img/Whirlpool.gif" class="spin"/>');
        }
    }).done(function(data) {
        promise.resolve(data);
    }).always(function() {
        //kill spinner
        $(id).find('.spin').fadeOut(3000, function() {
            $(this).remove();
        });
    }).fail(function() {
        //handle failure
        promise.reject();
    });
    return promise.promise();
}

$(document).ready(function() {
    $('#fullpage').fullpage({
        //Navigation
        menu: '#myMenu',
        lockAnchors: false,
        anchors: ['firstPage', 'secondPage', 'thirdPage', 'fourthPage', 'fifthPage', 'sixthPage', 'seventhPage', 'eighthPage', 'ninthPage', 'tenthPage', 'eleventhPage', 'twelfthPage', 'thirteenthPage','fourteenthPage'],
        navigation: true,
        navigationPosition: 'right',
        showActiveTooltip: false,
        navigationTooltips: ['Home', 'About', 'Undergraduate', 'Graduate', 'Undergrad Minors', 'Employment', 'Job Location', 'Coop Locations', 'Our People', 'Faculty Research By Interest', 'Faculty Research By Faculty', 'Student Resources', 'Contact Us','footer'],
        slidesNavigation: true,
        slidesNavPosition: 'bottom',

        //Scrolling
        css3: true,
        scrollingSpeed: 700,
        autoScrolling: true,
        fitToSection: true,
        fitToSectionDelay: 1000,
        scrollBar: false,
        easing: 'easeInOutCubic',
        easingcss3: 'ease',
        loopBottom: false,
        loopTop: false,
        loopHorizontal: true,
        continuousVertical: false,
        continuousHorizontal: false,
        scrollHorizontally: false,
        interlockedSlides: false,
        resetSliders: false,
        fadingEffect: false,
        normalScrollElements: '#element1, .element2',
        scrollOverflow: false,
        scrollOverflowOptions: null,
        touchSensitivity: 15,
        normalScrollElementTouchThreshold: 5,
        bigSectionsDestination: null,

        //Accessibility
        keyboardScrolling: true,
        animateAnchor: true,
        recordHistory: true,

        //Design
        controlArrows: true,
        verticalCentered: true,
        sectionsColor: ['#ccc', '#fff'],
        paddingTop: '3em',
        paddingBottom: '10px',
        fixedElements: '#header, .footer',
        responsiveWidth: 0,
        responsiveHeight: 0,
        responsiveSlides: false,

        //Custom selectors
        sectionSelector: '.section',
        slideSelector: '.slide',

        //events
        onLeave: function(index, nextIndex, direction) {},
        afterLoad: function(anchorLink, index) {},
        afterRender: function() {},
        afterResize: function() {},
        afterResponsive: function(isResponsive) {},
        afterSlideLoad: function(anchorLink, index, slideAnchor, slideIndex) {},
        onSlideLeave: function(anchorLink, index, slideIndex, direction, nextSlideIndex) {}
    });
});
