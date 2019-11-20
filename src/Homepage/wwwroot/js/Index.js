// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.
$(document).ready(function () {
    var myDiv = $(".media");
    myDiv.mouseover(function () {
        myDiv.clearQueue();
        $(this).animate({ top: "-5px" }, 120);
    });
    myDiv.mouseleave(function () {
        myDiv.clearQueue();
        $(this).animate({ top: "0" }, 120);
    });
    myDiv.mousedown(function () {
        myDiv.clearQueue();
        $(this).animate({ top: "0" }, 90);
    });
    myDiv.mouseup(function () {
        myDiv.clearQueue();
        $(this).animate({ top: "-5px" }, 90);
    });
});