$(document).ready(function (e) {
    /* Register event handler for delete links */
    $('body').on('click', 'a.delete', function (e) {
        e.preventDefault();

        // Retrieve postID associated with selected post
        var postID = $(e.target).closest('.post').attr('data-post-id');

        // Load delete screen
        $.get('/Post/Delete', { id : postID }, function (data, status) {
            $('.modal-dialog').html(data);
            $('#modal-container').modal('show');
        });
    });
});

/**
 * Load new post and add it to the page
 */
function loadPost(postID) {
    $.get("/Post/View", { id : postID }, function(data, status) {
        $('.posts').append($(data));
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