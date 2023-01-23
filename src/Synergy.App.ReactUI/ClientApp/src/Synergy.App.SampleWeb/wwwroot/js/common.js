var EncryptionKey = "SynergyKey";
function GetStack() {
    return new Stack();
}
function TrimText(text, size) {
    if (text.length > size - 3) {
        return text.substring(0, size - 4) + '...';
    }
    else {
        return text;
    }
}
window.OnPhotoError = (obj) => {
    obj.onerror = null;
    obj.src = '/images/profile.jpg';

};
window.OnPreviewError = (obj) => {
    obj.onerror = null;
    obj.src = '/images/nopreview.png';

};

window.OnDocumentError = (obj) => {
    obj.onerror = null;
    obj.src = '/images/document.png';

};
window.OnLogoError = (obj) => {
    console.log($(obj).parent());

    // obj.src = '/images/logo.png';
    $(obj).parent.css('padding-top', '15px');
    obj.onerror = null;
    $(obj).hide();


};
function InitializeContePlaceHolder() {

    $('.content-editable').each(function (i, obj) {
        var t = $(this);
        if (t.html().trim() === '') {
            t.html(t.data('placeholder'));
        }
    });



}
function ContentEditableBlur(e) {
    var t = $(e);
    var value = t.html().trim();
    if (value === '') {
        t.html(t.data('placeholder'));
        t.addClass('placeholder');
    }
    else {
        t.removeClass('placeholder');
    }
}
function ContentEditableFocus(e) {
    var t = $(e);
    var value = t.html().trim();
    if (value == t.data('placeholder')) {
        t.html('');
        t.removeClass('placeholder');
    }
}

function GenerateGuid() {
    //return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
    //    var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
    //    return v.toString(16);
    //});

    //var timestamp = (new Date().getTime() / 1000 | 0).toString(16);
    //return timestamp + 'xxxxxxxxxxxxxxxx'.replace(/[x]/g, function () {
    //    return (Math.random() * 16 | 0).toString(16);
    //}).toLowerCase();
    function _p8(s) {
        var p = (Math.random().toString(16) + "000000000").substr(2, 8);
        return s ? "-" + p.substr(0, 4) + "-" + p.substr(4, 4) : p;
    }
    return _p8() + _p8(true) + _p8(true) + _p8();
}
function ValidateHindi(str) {
    var hasNonHindi =str.split("").filter(function (char) {
        var charCode = char.charCodeAt(); return charCode < 2309 && charCode > 2361 ;
    }).length ;
}
function UtilityAjax(action, query) {
    var url = "../../utility/" + action + "?" + query;
    var ret = "";
    $.ajax({
        url: url,
        type: 'GET',
        cache: false,
        async: false,
        success: function (data) {
            ret = data;
        },
        error: function (errData) {
            OnError(errData);

        }
    });
    return ret;
}
class Stack {

    constructor() {
        this.items = [];
    }

    push(element) {
        this.items.push(element);
    }
    pop() {
        if (this.items.length == 0)
            return null;
        return this.items.pop();
    }
    peek(index) {
        if (index === null || index === undefined) {
            index = 0;
        }
        if (this.items.length <= index)
            return null;
        return this.items[this.items.length - index - 1];
    }
    isEmpty() {
        return this.items.length == 0;
    }
}
