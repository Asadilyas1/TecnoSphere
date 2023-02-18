$(document).ready(function () {
    LoadGallery();
});
function LoadGallery() {
    $.ajax({
        type: "GET",
        url: "/Dashboard/LoadGallery",
        data: {

        },
        success: function (res) {
            $("#LoadGallery").html(res);
        },
        error: function (res) {
            alert(res);
        }
    });
}

new Dropzone("#uploader", {
    maxFilesize: 2, // MB
    maxFiles: 20,
    preventDuplicates: true,
    maxfilesexceeded: function (file) {
        this.removeAllFiles();
        this.addFile(file);
    },
    init: function () {
        this.on("success", function (event) {
            while (this.files.length > this.options.maxFiles) {
                this.removeFile(this.files[0]);
            }
            LoadGallery();
        });
    }
});
Dropzone.autoDiscover = false;

Dropzone.prototype.isFileExist = function (file) {
    var i;
    if (this.files.length > 0) {
        for (i = 0; i < this.files.length; i++) {
            if (this.files[i].name === file.name
                && this.files[i].size === file.size
                && this.files[i].lastModifiedDate.toString() === file.lastModifiedDate.toString()) {
                return true;
            }
        }
    }
    return false;
};

Dropzone.prototype.addFile = function (file) {
    file.upload = {
        progress: 0,
        total: file.size,
        bytesSent: 0
    };
    if (this.options.preventDuplicates && this.isFileExist(file)) {
        alert(this.options.dictDuplicateFile);
        return;
    }
    this.files.push(file);
    file.status = Dropzone.ADDED;
    this.emit("addedfile", file);
    this._enqueueThumbnail(file);
    return this.accept(file, (function (_this) {
        return function (error) {
            if (error) {
                file.accepted = false;
                _this._errorProcessing([file], error);
            } else {
                file.accepted = true;
                if (_this.options.autoQueue) {
                    _this.enqueueFile(file);
                }
            }
            return _this._updateMaxFilesReachedClass();
        };
    })(this));
};

function Delete(id) {
    swal({
        title: "Are you sure?",
        text: "You will not be able to recover this imaginary file!",
        type: "warning",
        buttons: true,
        dangerMode: true,
    })
        .then((willDelete) => {
            if (willDelete) {
                $.ajax({
                    url: "/Dashboard/DeleteGallery",
                    type: "POST",
                    data: {
                        id: id
                    },
                    dataType: "json",
                    success: function () {
                        swal("Done!", "It was succesfully deleted!", "success");
                        LoadGallery();
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        swal("Error deleting!", "Please try again", "error");
                    }
                });
            } else {
                swal("Your imaginary file is safe!");
            }
        });
}
