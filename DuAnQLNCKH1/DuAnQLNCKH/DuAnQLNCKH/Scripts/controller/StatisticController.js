

$(document).ready(function () {

    

    function Contains(text_one, text_two) {
        if (text_one.indexOf(text_two) != -1)
            return true;
    }

// statis lecture

    $("#Search").keyup(function () {
        var searchText = $("#Search").val().toLowerCase();
       
        $(".Search").each(function () {
            var id = $(this).find(".nametd");
           
            if (Contains($(id).text().toLowerCase(), searchText)) {
                $(this).show();
            }
            else {
                $(this).hide();
            }
        })
    })
 
    $("#StatisticYear").change(function () {
        var searchText = $("#StatisticYear").val();

        $(".Search").each(function () {
            var id = $(this).find(".Year").text().substr(6, 4);

            if (!Contains(id, searchText)) {
                $(this).hide();
            }
            else {
                $(this).show();
            }
        })
    })

    // Tìm kiếm giảng viên

    $("#buttonSearch").click(function () {


         
        //var a = $("#IdP option:selected").val();
        
        if ($("#IdTy").text() != "--Tất cả loại đề tài--") {
            var a = $("#IdTy option:selected").val();
            $("#IdTya").val(a);
        }
        if ($("#IdF").text()!="--Tất cả khoa--") {
            var b = $("#IdF option:selected").val();
            $("#IdFa").val(b);
        }
        if ($("#IdLe").text() != "--Tất cả giảng viên--") {
            var c = $("#IdLe option:selected").val();
            $("#IdLea").val(c);
        }
        var dateSt = $("#DateSt").val();
        var dateEnd = $("#DateEnd").val();

        var dateEnd1 = new Date(dateEnd);
        var dateSt1 = new Date(dateSt);
        $("#DateSt1").val(dateSt);
        $("#DateEnd1").val(dateEnd);
      
        var IdTySearch = $("#IdTy").val();
        var IdFSearch = $("#IdF").val();
        var IdLeSearch = $("#IdLe").val();
        $(".Search").each(function () {

            var id = $(this).find(".Year1").val();
            var IdTy = $(this).find(".IdTy").val();
            var IdF = $(this).find(".IdFacu1").val();
            var IdLe = $(this).find(".IdLe1").val();
            //var pointBody = $(this).find(".pointBody");
            if (IdTySearch == "") {
                if (IdFSearch == "") {
                    if (IdLeSearch == "") {
                        $("#pointHeader").hide();
                        $(".pointBody").hide();
                        if (dateSt1 < new Date(id) && dateEnd1 > new Date(id)) {

                            $(this).show();
                        }
                        else {
                            $(this).hide();
                        }
                    }
                    else {
                        $("#pointHeader").show();
                        $(".pointBody").show();
                        if (dateSt1 < new Date(id) && dateEnd1 > new Date(id) && IdLe== IdLeSearch) {

                            $(this).show();
                        }
                        else {
                            $(this).hide();
                        }
                    }
                    
                }
                else {
                    if (IdLeSearch == "") {
                        $("#pointHeader").hide();
                        $(".pointBody").hide();
                        if (dateSt1 < new Date(id) && dateEnd1 > new Date(id) && IdF == IdFSearch) {

                            $(this).show();
                        }
                        else {
                            $(this).hide();
                        }
                    }
                    else {
                        $("#pointHeader").show();
                        $(".pointBody").show();
                        if (dateSt1 < new Date(id) && dateEnd1 > new Date(id) && IdLe == IdLeSearch && IdF == IdFSearch) {

                            $(this).show();
                        }
                        else {
                            $(this).hide();
                        }
                    }
                }
            }
            else {

                if (IdFSearch == "") {
                    if (IdLeSearch == "") {
                        $("#pointHeader").hide();
                        $(".pointBody").hide();
                        if (dateSt1 < new Date(id) && dateEnd1 > new Date(id) && IdTy == IdTySearch) {

                            $(this).show();
                        }
                        else {
                            $(this).hide();
                        }
                    }
                    else {
                        $("#pointHeader").show();
                        $(".pointBody").show();
                        if (dateSt1 < new Date(id) && dateEnd1 > new Date(id) && IdLe == IdLeSearch && IdTy == IdTySearch) {

                            $(this).show();
                        }
                        else {
                            $(this).hide();
                        }
                    }

                }
                else {
                    if (IdLeSearch == "") {
                        $("#pointHeader").hide();
                        $(".pointBody").hide();
                        if (dateSt1 < new Date(id) && dateEnd1 > new Date(id) && IdF == IdFSearch && IdTy == IdTySearch) {

                            $(this).show();
                        }
                        else {
                            $(this).hide();
                        }
                    }
                    else {
                        $("#pointHeader").show();
                        $(".pointBody").show();
                        if (dateSt1 < new Date(id) && dateEnd1 > new Date(id) && IdLe == IdLeSearch && IdF == IdFSearch && IdTy == IdTySearch) {

                            $(this).show();
                        }
                        else {
                            $(this).hide();
                        }
                    }
                }
            }
            
        })


    })

   // Tìm kiếm sinh viên

    $("#buttonSearchStu").click(function () {



        //var a = $("#IdP option:selected").val();

        if ($("#IdTyStu").text() != "--Tất cả loại đề tài--") {
            var a = $("#IdTyStu option:selected").val();
            $("#IdTyaStu").val(a);
        }
        if ($("#IdFStu").text() != "--Tất cả khoa--") {
            var b = $("#IdFStu option:selected").val();
            $("#IdFaStu").val(b);
        }
        if ($("#IdSV").text() != "--Tất cả sinh viên--") {
            var c = $("#IdSV option:selected").val();
            $("#IdLeaStu").val(c);
        }
        var dateSt = $("#DateStStu").val();
        var dateEnd = $("#DateEndStu").val();

        var dateEnd1 = new Date(dateEnd);
        var dateSt1 = new Date(dateSt);
        $("#DateSt1Stu").val(dateSt);
        $("#DateEnd1Stu").val(dateEnd);

        var IdTySearch = $("#IdTyStu").val();
        var IdFSearch = $("#IdFStu").val();
        var IdLeSearch = $("#IdSV").val();
        $(".SearchStu").each(function () {

            var id = $(this).find(".Year1Stu").val();
            var IdTy = $(this).find(".IdTyStu").val();
            var IdF = $(this).find(".IdFacu1Stu").val();
            var IdLe = $(this).find(".IdSV1").val();
            //var pointBody = $(this).find(".pointBody");
            if (IdTySearch == "") {
                if (IdFSearch == "") {
                    if (IdLeSearch == "") {
                        if (dateSt1 < new Date(id) && dateEnd1 > new Date(id)) {

                            $(this).show();
                        }
                        else {
                            $(this).hide();
                        }
                    }
                    else {
                        $("#pointHeaderStu").show();
                        $(".pointBodyStu").show();
                        if (dateSt1 < new Date(id) && dateEnd1 > new Date(id) && IdLe == IdLeSearch) {

                            $(this).show();
                        }
                        else {
                            $(this).hide();
                        }
                    }

                }
                else {
                    if (IdLeSearch == "") {
                        if (dateSt1 < new Date(id) && dateEnd1 > new Date(id) && IdF == IdFSearch) {

                            $(this).show();
                        }
                        else {
                            $(this).hide();
                        }
                    }
                    else {
                        $("#pointHeaderStu").show();
                        $(".pointBodyStu").show();
                        if (dateSt1 < new Date(id) && dateEnd1 > new Date(id) && IdLe == IdLeSearch && IdF == IdFSearch) {

                            $(this).show();
                        }
                        else {
                            $(this).hide();
                        }
                    }
                }
            }
            else {

                if (IdFSearch == "") {
                    if (IdLeSearch == "") {
                        if (dateSt1 < new Date(id) && dateEnd1 > new Date(id) && IdTy == IdTySearch) {

                            $(this).show();
                        }
                        else {
                            $(this).hide();
                        }
                    }
                    else {
                        $("#pointHeaderStu").show();
                        $(".pointBodyStu").show();
                        if (dateSt1 < new Date(id) && dateEnd1 > new Date(id) && IdLe == IdLeSearch && IdTy == IdTySearch) {

                            $(this).show();
                        }
                        else {
                            $(this).hide();
                        }
                    }

                }
                else {
                    if (IdLeSearch == "") {
                        if (dateSt1 < new Date(id) && dateEnd1 > new Date(id) && IdF == IdFSearch && IdTy == IdTySearch) {

                            $(this).show();
                        }
                        else {
                            $(this).hide();
                        }
                    }
                    else {
                        $("#pointHeaderStu").show();
                        $(".pointBodyStu").show();
                        if (dateSt1 < new Date(id) && dateEnd1 > new Date(id) && IdLe == IdLeSearch && IdF == IdFSearch && IdTy == IdTySearch) {

                            $(this).show();
                        }
                        else {
                            $(this).hide();
                        }
                    }
                }
            }

        })


    })

      

    $("#SearchStudent").keyup(function () {
        var searchText = $("#SearchStudent").val().toLowerCase();

        $(".SearchStu").each(function () {
            var id = $(this).find(".namestudent");
            if (!Contains($(id).text().toLowerCase(), searchText)) {
                $(this).hide();
            }
            else {
                $(this).show();
            }
        })

    })
 
})