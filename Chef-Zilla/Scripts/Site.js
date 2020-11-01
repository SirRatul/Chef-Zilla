var current = location.pathname;
$('.nav-sidebar li a').each(function() {
    var $this = $(this);
    if ($this.attr('href').indexOf(current) !== -1) {
        $this.addClass('active');
    }
});
