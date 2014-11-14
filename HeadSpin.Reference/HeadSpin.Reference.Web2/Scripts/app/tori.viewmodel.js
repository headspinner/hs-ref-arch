function ToriViewModel(app, dataModel) {
    var self = this;

    self.Name = ko.observable("");
    self.Bday = ko.observable("");

    Sammy(function () {
        this.get('#tori', function () {
            // Make a call to the protected Web API by passing in a Bearer Authorization Header
            $.ajax({
                method: 'get',
                url: app.dataModel.toriInfoUrl,
                contentType: "application/json; charset=utf-8",
                headers: {
                    'Authorization': 'Bearer ' + app.dataModel.getAccessToken()
                },
                success: function (data) {
                    self.Name(data.name);
                    self.Bday(data.bday);
                    console.log(data);
                }
            });
        });
        this.get('/', function () { this.app.runRoute('get', '#tori') });
    });

    self.Save = function () {
        console.log('saved...');
    };

    //self.Save = function () {
    //    try {


    //        // Make a call to the protected Web API by passing in a Bearer Authorization Header
    //        $.ajax({
    //            method: 'post',
    //            url: app.dataModel.toriInfoUrl,
    //            contentType: "application/json; charset=utf-8",
    //            data: { Name: self.Name() },
    //            headers: {
    //                'Authorization': 'Bearer ' + app.dataModel.getAccessToken()
    //            },
    //            success: function (data) {
    //                console.log(data);
    //            }
    //        });
    //    }
    //    catch (err) {
    //        console.log(err);
    //    }

    //};
     
    

    return self;
}

app.addViewModel({
    name: "Tori",
    bindingMemberName: "tori",
    factory: ToriViewModel
});
