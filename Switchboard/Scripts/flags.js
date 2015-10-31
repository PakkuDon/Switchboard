$(document).ready(function () {
    console.log($('.flag-reponse-form'));
    $('.flag-reponse-form').submit(function (e) {
        e.preventDefault();

        // Retrieve associated flag ID and DOM elements
        var $form = $(e.target);
        var $flagItem = $form.closest('.flag-item');
        var flagID = $flagItem.attr('data-flag-id');

        // Send response to resolve associated flag and update flag item
        $.post($form.attr('action'), $form.serialize(), function (data, status) {
            $('.flag-item[data-flag-id=' + flagID + ']').replaceWith(data);
        });
    });
});