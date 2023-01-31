
window.OnPhotoError = (obj) => {
    obj.onerror = null;
    obj.src = '/images/profile.jpg';

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