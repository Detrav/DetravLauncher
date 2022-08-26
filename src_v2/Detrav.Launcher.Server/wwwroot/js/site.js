// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


$(document).ready(function () {

    $(".cb-tag-into-product").change(function () {
        var productId = $(this).attr("data-product");
        var tagId = $(this).attr("data-tag");
        if (this.checked) {
            fetch(`/api/AdminActions/AddTagToProduct?productId=${productId}&tagId=${tagId}`, { method: "POST"})
                .then((response) => {
                    if (response.ok) {
                        // TODO?
                    }
                });
        }
        else {
            fetch(`/api/AdminActions/RemoveTagFromProduct?productId=${productId}&tagId=${tagId}`, { method: "POST" })
                .then((response) => {
                    if (response.ok) {
                        // TODO?
                    }
                });
        }
    })
});