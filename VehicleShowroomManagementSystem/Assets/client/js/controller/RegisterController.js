var register = {
    init: function () {
        register.registerEvents();
      
    },
    registerEvents: function () {
       
        $('#loadImg').change(function (evt) {
            if (this.files && this.files[0]) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    $('#showImg').attr('src', e.target.result);
                }
                reader.readAsDataURL(this.files[0]);
            }

        });
    },
   
}
register.init();