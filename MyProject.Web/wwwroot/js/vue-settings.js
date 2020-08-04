// Vue app.
var app = new Vue({
    el: '#app',
    data: {
        change_password: false,
        password: null
    },

    methods: {

        saveForm: function () {
            var me = this;
            // Validate all required fields
            me.$validator.validateAll().then(function (result) {
                if (result) {
                    document.getElementById("saveForm").submit();
                }
            });
        }
    }

});