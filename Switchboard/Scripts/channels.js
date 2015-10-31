$(document).ready(function (e) {
    // Highlight current channel link
    $('.sidebar li a[href$=\'' + location.pathname + '\']').addClass('active');

    /* Register event handler for delete links */
    $('body').on('click', 'a.delete', function (e) {
        e.preventDefault();

        // Retrieve postID associated with selected post
        var postID = $(e.target).closest('.post').attr('data-post-id');

        // Load delete screen
        $.get('/Post/Delete', { id: postID }, function (data, status) {
            $('.modal-dialog').html(data);
            $('#modal-container').modal('show');
        });
    });

    /* Register event handler for edit links */
    $('body').on('click', 'a.edit', function (e) {
        e.preventDefault();

        // Retrieve postID associated with selected post
        var postID = $(e.target).closest('.post').attr('data-post-id');
        // Load edit screen
        $.get("/Post/Edit", { id: postID }, function (data, status) {
            $('.post[data-post-id=' + postID + ']').replaceWith(data);
        });
    });

    /* Register event handler for edit-form cancel button */
    $('body').on('click', '.edit-form .cancel', function (e) {
        // Retrieve postID associated with selected post
        var postID = $(e.target).closest('.post').attr('data-post-id');
        // Render post again
        $.get("/Post/View", { id: postID }, function (data, status) {
            $('.post[data-post-id=' + postID + ']').replaceWith(data);
        });
    });

    /* Register event handler for edit submit action */
    $('body').on('submit', '.edit-form form', function (e) {
        e.preventDefault();

        var postID = $(e.target).closest('.post').attr('data-post-id');
        // Send request to edit selected post
        var $form = $(e.target);
        $.post($form.attr('action'), $form.serialize(), function (data, status) {
            $('.post[data-post-id=' + postID + ']').replaceWith(data);
        });
    });

    /* Register event handler for undelete action */
    $('body').on('click', 'a.undelete', function (e) {
        e.preventDefault();

        // Retrieve postID associated with selected post
        var postID = $(e.target).closest('.post').attr('data-post-id');

        // Send request to undo delete action on post
        $.post('/Post/Undelete', { id: postID }, function (data, status) {
            // Render updated post
            $('.post[data-post-id=' + postID + ']').replaceWith(data);
        });
    });

    /* Register event handler for flag links */
    $('body').on('click', 'a.flag', function (e) {
        e.preventDefault();

        // Retrieve postID associated with selected post
        var postID = $(e.target).closest('.post').attr('data-post-id');

        // Load flag form
        $.get('/Flag/Create', { postID: postID }, function (data, status) {
            // Render updated post
            $('.modal-dialog').html(data);
            $('#modal-container').modal('show');
        });
    });
});

/**
 * Load new post and add it to the page
 */
function loadPost(postID) {
    $.get("/Post/View", { id: postID }, function (data, status) {
        var $post = $(data);
        $('.posts').append($post);

        // Highlight new post for a few seconds
        $post.addClass("new");
        setTimeout(function () {
            $post.removeClass("new");
        }, 3000);
    });
}

/**
 * Removes element containing post with given ID from DOM
 */
function removePost(postID) {
    var $post = $('.post[data-post-id=' + postID + ']');
    $post.slideUp(500, function () {
        $post.remove();
    });
}

/** 
 * Fetch new contents of post with given ID 
 */
function updatePost(postID) {
    var $post = $('.post[data-post-id=' + postID + ']');
    $.get('/Post/View', { id : postID }, function (data, status) {
        $post.replaceWith(data);
    });
}