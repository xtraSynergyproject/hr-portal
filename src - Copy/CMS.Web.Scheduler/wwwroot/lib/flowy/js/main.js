var LoadFlowY = function () {
  
    var rightcard = false;
    var tempblock;
    var tempblock2;
    //document.getElementById("blocklist").innerHTML = '<div class="blockelem create-flowy noselect"><input type="hidden" name="blockelemtype" class="blockelemtype" value="1"><div class="grabme"><i class="fad fa-sitemap"></i></div><div class="blockin">                  <div class="blockico"><span></span><i class="fad fa-sitemap"></i></div><div class="blocktext">                        <p class="blocktitle">New visitor</p><p class="blockdesc">Triggers when somebody visits a specified page</p>        </div></div></div><div class="blockelem create-flowy noselect"><input type="hidden" name="blockelemtype" class="blockelemtype" value="2"><div class="grabme"><i class="fad fa-sitemap"></i></div><div class="blockin">                    <div class="blockico"><span></span><i class="fad fa-sitemap"></i></div><div class="blocktext">                        <p class="blocktitle">Action is performed</p><p class="blockdesc">Triggers when somebody performs a specified action</p></div></div></div><div class="blockelem create-flowy noselect"><input type="hidden" name="blockelemtype" class="blockelemtype" value="3"><div class="grabme"><i class="fad fa-sitemap"></i></div><div class="blockin">                    <div class="blockico"><span></span><i class="fad fa-sitemap"></i></div><div class="blocktext">                        <p class="blocktitle">Time has passed</p><p class="blockdesc">Triggers after a specified amount of time</p>          </div></div></div><div class="blockelem create-flowy noselect"><input type="hidden" name="blockelemtype" class="blockelemtype" value="4"><div class="grabme"><i class="fad fa-sitemap"></i></div><div class="blockin">                    <div class="blockico"><span></span><i class="fad fa-sitemap"></i></div><div class="blocktext">                        <p class="blocktitle">Error prompt</p><p class="blockdesc">Triggers when a specified error happens</p>              </div></div></div>';
    flowy(document.getElementById("canvas"), drag, release, snapping);
    function addEventListenerMulti(type, listener, capture, selector) {       
        var nodes = document.querySelectorAll(selector);
        for (var i = 0; i < nodes.length; i++) {
            nodes[i].addEventListener(type, listener, capture);
        }
    }
    var i = 2;
    function snapping(drag, first) {
        var id = GenerateGuid();
        var grab = drag.querySelector(".grabme");
        grab.parentNode.removeChild(grab);
        var blockin = drag.querySelector(".blockin");
        blockin.parentNode.removeChild(blockin);
        if (drag.querySelector(".blockelemtype").value == "Email") {
            drag.innerHTML += "<div class='blockyleft'><i class='fad fa-sitemap'></i><p class='blockyname'>Email</p></div><div class='blockyright'><i class='fad fa-sitemap'></i></div><div class='blockydiv'></div><div class='blockyinfo'>Email</div><input type='hidden' name='id' id='id' value='" + id +"'>";
        } else if (drag.querySelector(".blockelemtype").value == "AdhocTask") {
            drag.innerHTML += "<div class='blockyleft'><img src='assets/actionblue.svg'><p class='blockyname'>Adhoc Task</p></div><div class='blockyright'><img src='assets/more.svg'></div><div class='blockydiv'></div><div class='blockyinfo'>Adhoc Task</div><input type='hidden' name='id' id='id' value='" + id +"'>";
        }
        else if (drag.querySelector(".blockelemtype").value == "StepTask") {
            var block = drag.querySelector(".blockid");
            drag.innerHTML += "<div class='blockyleft'><i class='fad fa-tasks-alt'></i><p class='blockyname'>Step Task</p></div><div class='blockyright'><i class='far fa-ellipsis-h'></i></div><div class='blockydiv'></div><div class='blockyinfo'>Step Task</div><input type='hidden' name='id' id='id' value='" + id +"'><input type='hidden' name='blockProp' id='StepTask_block_" + block.value + "' value='0'>";
        }
        else if (drag.querySelector(".blockelemtype").value == "ExecutionScript") {
            var block = drag.querySelector(".blockid");
            drag.innerHTML += "<div class='blockyleft'><i class='fal fa-tablet-rugged'></i><p class='blockyname'>Execution Script</p></div><div class='blockyright'><i class='far fa-ellipsis-h'></i></div><div class='blockydiv'></div><div class='blockyinfo'>Execution Script</div><input type='hidden' name='id' id='id' value='" + id +"'><input type='hidden' name='blockProp' id='ExeScript_block_" + block.value + "' value='0'>";
        }
        else if (drag.querySelector(".blockelemtype").value == "DecisionScriptTrue") {
          
            var block = drag.querySelector(".blockid");
            drag.innerHTML += "<div class='blockyleft'><i class='fal fa-tablet-rugged'></i><p class='blockyname'>True</p></div><div class='blockyright'><i class='far fa-ellipsis-h'></i></div><div class='blockydiv'></div><div class='blockyinfo'>True</div><input type='hidden' name='id' id='id' value='" + id + "'><input type='hidden' name='blockProp' id='ExeScript_block_" + block.value + "' value='0'>";
        }
        else if (drag.querySelector(".blockelemtype").value == "DecisionScript") {
            var block = drag.querySelector(".blockid");
            drag.innerHTML += "<div class='blockyleft'><i class='fal fa-business-time'></i><p class='blockyname'>Decision Script</p></div><div class='blockyright'><i class='far fa-ellipsis-h'></i></div><div class='blockydiv'></div><div class='blockyinfo'>Step Task</div><input type='hidden' name='id' id='id' value='" + id +"'><input type='hidden' name='blockProp' id='DecisionScript_block_" + block.value + "' value='0'>";
            //var tr = $('#descisionTrueBlock');
            //flowy(document.getElementById("canvas"), tr, release, snapping);
           // snapping(tr, false);
            //for ( i; i < 3; i++)
            //{
            //    drag.appendChild(grab)
            //    drag.appendChild(blockin)
            //    snapping(drag, false);
            //}
           
        }
        else if (drag.querySelector(".blockelemtype").value == "ProcessDesign") {
            drag.innerHTML += "<div class='blockyleft'><img src='assets/timeblue.svg'><p class='blockyname'>ProcessDesign</p></div><div class='blockyright'><img src='assets/more.svg'></div><div class='blockydiv'></div><div class='blockyinfo'>ProcessDesign</div><input type='hidden' name='id' id='id' value='" + id +"'>";
        } else if (drag.querySelector(".blockelemtype").value == "BusinessLogic") {
            drag.innerHTML += "<div class='blockyleft'><img src='assets/errorblue.svg'><p class='blockyname'>BusinessLogict</p></div><div class='blockyright'><img src='assets/more.svg'></div><div class='blockydiv'></div><div class='blockyinfo'>BusinessLogic</div><input type='hidden' name='id' id='id' value='" + id +"'>";
        } else if (drag.querySelector(".blockelemtype").value == "BusinessExecution") {
            drag.innerHTML += "<div class='blockyleft'><img src='assets/databaseorange.svg'><p class='blockyname'>BusinessExecution</p></div><div class='blockyright'><img src='assets/more.svg'></div><div class='blockydiv'></div><div class='blockyinfo'>BusinessExecution</div><input type='hidden' name='id' id='id' value='" + id +"'>";
        } else if (drag.querySelector(".blockelemtype").value == "CompleteEvent") {
            drag.innerHTML += "<div class='blockyleft'><img src='assets/databaseorange.svg'><p class='blockyname'>CompleteEvent</p></div><div class='blockyright'><img src='assets/more.svg'></div><div class='blockydiv'></div><div class='blockyinfo'>CompleteEvent</div><input type='hidden' name='id' id='id' value='" + id +"'>";

        }//} else if (drag.querySelector(".blockelemtype").value == "7") {
        //    drag.innerHTML += "<div class='blockyleft'><img src='assets/actionorange.svg'><p class='blockyname'>Perform an action</p></div><div class='blockyright'><img src='assets/more.svg'></div><div class='blockydiv'></div><div class='blockyinfo'>Perform <span>Action 1</span></div>";
        //} else if (drag.querySelector(".blockelemtype").value == "8") {
        //    drag.innerHTML += "<div class='blockyleft'><img src='assets/twitterorange.svg'><p class='blockyname'>Make a tweet</p></div><div class='blockyright'><img src='assets/more.svg'></div><div class='blockydiv'></div><div class='blockyinfo'>Tweet <span>Query 1</span> with the account </div>";
        //} else if (drag.querySelector(".blockelemtype").value == "9") {
        //    drag.innerHTML += "<div class='blockyleft'><img src='assets/logred.svg'><p class='blockyname'>Add new log entry</p></div><div class='blockyright'><img src='assets/more.svg'></div><div class='blockydiv'></div><div class='blockyinfo'>Add new <span>success</span> log entry</div>";
        //} else if (drag.querySelector(".blockelemtype").value == "10") {
        //    drag.innerHTML += "<div class='blockyleft'><img src='assets/logred.svg'><p class='blockyname'>Update logs</p></div><div class='blockyright'><img src='assets/more.svg'></div><div class='blockydiv'></div><div class='blockyinfo'>Edit <span>Log Entry 1</span></div>";
        //} else if (drag.querySelector(".blockelemtype").value == "11") {
        //    drag.innerHTML += "<div class='blockyleft'><img src='assets/errorred.svg'><p class='blockyname'>Prompt an error</p></div><div class='blockyright'><img src='assets/more.svg'></div><div class='blockydiv'></div><div class='blockyinfo'>Trigger <span>Error 1</span></div>";
        //}
        return true;
    }
    function drag(block) {
        block.classList.add("blockdisabled");
        tempblock2 = block;
    }
    function release() {
        if (tempblock2) {
            tempblock2.classList.remove("blockdisabled");
        }
    }
    var disabledClick = function () {
        document.querySelector(".navactive").classList.add("navdisabled");
        document.querySelector(".navactive").classList.remove("navactive");
        this.classList.add("navactive");
        this.classList.remove("navdisabled");
        //if (this.getAttribute("id") == "triggers") {
        //    document.getElementById("blocklist").innerHTML = '<div class="blockelem create-flowy noselect"><input type="hidden" name="blockelemtype" class="blockelemtype" value="1"><div class="grabme"><i class="fad fa-arrows-alt"></i></div><div class="blockin">                  <div class="blockico"><span></span><i class="fad fa-arrows-alt"></i></div><div class="blocktext">                        <p class="blocktitle">Email</p><p class="blockdesc">Email</p>        </div></div></div><div class="blockelem create-flowy noselect"><input type="hidden" name="blockelemtype" class="blockelemtype" value="2"><div class="grabme"><img src="assets/grabme.svg"></div><div class="blockin">                    <div class="blockico"><span></span><img src="assets/action.svg"></div><div class="blocktext">                        <p class="blocktitle">Task</p><p class="blockdesc">Task</p></div></div></div><div class="blockelem create-flowy noselect"><input type="hidden" name="blockelemtype" class="blockelemtype" value="3"><div class="grabme"><img src="assets/grabme.svg"></div><div class="blockin">                    <div class="blockico"><span></span><img src="assets/time.svg"></div><div class="blocktext">                        <p class="blocktitle">Time has passed</p><p class="blockdesc">Triggers after a specified amount of time</p>          </div></div></div><div class="blockelem create-flowy noselect"><input type="hidden" name="blockelemtype" class="blockelemtype" value="4"><div class="grabme"><img src="assets/grabme.svg"></div><div class="blockin">                    <div class="blockico"><span></span><img src="assets/error.svg"></div><div class="blocktext">                        <p class="blocktitle">Error prompt</p><p class="blockdesc">Triggers when a specified error happens</p>              </div></div></div>';
        //} else if (this.getAttribute("id") == "actions") {
        //    document.getElementById("blocklist").innerHTML = '<div class="blockelem create-flowy noselect"><input type="hidden" name="blockelemtype" class="blockelemtype" value="5"><div class="grabme"><i class="fad fa-arrows-alt"></i></div><div class="blockin">                  <div class="blockico"><span></span><i class="fad fa-arrows-alt"></i></div><div class="blocktext">                        <p class="blocktitle">New database entry</p><p class="blockdesc">Adds a new entry to a specified database</p>        </div></div></div><div class="blockelem create-flowy noselect"><input type="hidden" name="blockelemtype" class="blockelemtype" value="6"><div class="grabme"><img src="assets/grabme.svg"></div><div class="blockin">                  <div class="blockico"><span></span><img src="assets/database.svg"></div><div class="blocktext">                        <p class="blocktitle">Update database</p><p class="blockdesc">Edits and deletes database entries and properties</p>        </div></div></div><div class="blockelem create-flowy noselect"><input type="hidden" name="blockelemtype" class="blockelemtype" value="7"><div class="grabme"><img src="assets/grabme.svg"></div><div class="blockin">                  <div class="blockico"><span></span><img src="assets/action.svg"></div><div class="blocktext">                        <p class="blocktitle">Perform an action</p><p class="blockdesc">Performs or edits a specified action</p>        </div></div></div><div class="blockelem create-flowy noselect"><input type="hidden" name="blockelemtype" class="blockelemtype" value="8"><div class="grabme"><img src="assets/grabme.svg"></div><div class="blockin">                  <div class="blockico"><span></span><img src="assets/twitter.svg"></div><div class="blocktext">                        <p class="blocktitle">Make a tweet</p><p class="blockdesc">Makes a tweet with a specified query</p>        </div></div></div>';
        //} else if (this.getAttribute("id") == "loggers") {
        //    document.getElementById("blocklist").innerHTML = '<div class="blockelem create-flowy noselect"><input type="hidden" name="blockelemtype" class="blockelemtype" value="9"><div class="grabme"><i class="fad fa-arrows-alt"></i></div><div class="blockin">                  <div class="blockico"><span></span><i class="fad fa-arrows-alt"></i></div><div class="blocktext">                        <p class="blocktitle">Add new log entry</p><p class="blockdesc">Adds a new log entry to this project</p>        </div></div></div><div class="blockelem create-flowy noselect"><input type="hidden" name="blockelemtype" class="blockelemtype" value="10"><div class="grabme"><img src="assets/grabme.svg"></div><div class="blockin">                  <div class="blockico"><span></span><img src="assets/log.svg"></div><div class="blocktext">                        <p class="blocktitle">Update logs</p><p class="blockdesc">Edits and deletes log entries in this project</p>        </div></div></div><div class="blockelem create-flowy noselect"><input type="hidden" name="blockelemtype" class="blockelemtype" value="11"><div class="grabme"><img src="assets/grabme.svg"></div><div class="blockin">                  <div class="blockico"><span></span><img src="assets/error.svg"></div><div class="blocktext">                        <p class="blocktitle">Prompt an error</p><p class="blockdesc">Triggers a specified error</p>        </div></div></div>';
        //}
    }
    addEventListenerMulti("click", disabledClick, false, ".side");
    //document.getElementById("close").addEventListener("click", function () {
    //    document.getElementById("myCustomSidenavEditor").style.width = "0";
    //    $("#overlay").removeClass("overlay");
    //    if (rightcard) {
    //        rightcard = false;
    //        //document.getElementById("properties").classList.remove("expanded");
    //        //setTimeout(function () {
    //        //    document.getElementById("propwrap").classList.remove("itson");
    //        //}, 300);
    //        tempblock.classList.remove("selectedblock");
    //    }
    //});

    //document.getElementById("removeblock").addEventListener("click", function () {
    //    console.log(flowy.output());
    //    alert(flowy.output());
    //    alert(JSON.stringify(flowy.output()));

    //    //flowy.deleteBlocks();
    //});
    var aclick = false;
    var noinfo = false;
    var beginTouch = function (event) {
        aclick = true;
        noinfo = false;
        if (event.target.closest(".create-flowy")) {
            noinfo = true;
        }
    }
    var checkTouch = function (event) {
        aclick = false;
    }
   
    $("#close").on("click", function (e)
    {     
        document.getElementById("myCustomSidenavEditor").style.width = "0";
        $("#overlay").removeClass("overlay");
        if (rightcard) {
            rightcard = false;
            //document.getElementById("properties").classList.remove("expanded");
            //setTimeout(function () {
            //    document.getElementById("propwrap").classList.remove("itson");
            //}, 300);
            tempblock.classList.remove("selectedblock");
        }
    })
    var doneTouch = function (event) {      
        if (event.type === "mouseup" && aclick && !noinfo) {           
           //debugger
            //to be confirmed after testing
            if (!rightcard && event.target.closest(".block") && !event.target.closest(".block").classList.contains("dragging")) {
                var res = !event.target.closest(".block").classList.contains("dragging");
                var res1 = !rightcard && event.target.closest(".block");
                tempblock = event.target.closest(".block");
            rightcard = true;
            document.getElementById("properties").classList.add("expanded");
            document.getElementById("propwrap").classList.add("itson");
            var ll = document.getElementsByClassName('blockelem');
            ll.forEach(e => e.classList.remove("selectedblock"));
            //  $(".blockyleft").removeClass("selectedblock");.forEach(e => e.classList.remove("selectedblock"));
            tempblock.classList.add("selectedblock");
            // document.getElementById("properties").innerHTML="123";
            LoadProperty(tempblock);
            // console.log(tempblock);
           }
        }
    }
    addEventListener("mousedown", beginTouch, false);
    addEventListener("mousemove", checkTouch, false);
    addEventListener("mouseup", doneTouch, false);
    //addEventListener("click", closepopup, false);
    addEventListenerMulti("touchstart", beginTouch, false, ".block");


}

//function LoadFlowY()
//{
//    flowy(document.getElementById("canvas"), drag, release, snapping);
//}