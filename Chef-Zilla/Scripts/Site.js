var current = location.pathname;
console.log('Url' + current);
$('.nav-sidebar li a').each(function() {
    var $this = $(this);
    console.log('Value' + $this.attr('href').indexOf(current));
    if ($this.attr('href').indexOf(current) !== -1) {
        $this.addClass('active');
    }
});
