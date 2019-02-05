"use strict"

b4w.register("TeeShirt_main", function (exports, require) {


    var m_app = require("app");
    var m_cfg = require("config");
    var m_data = require("data");
    var m_preloader = require("preloader");
    var m_ver = require("version");

    var DEBUG = (m_ver.type() == "DEBUG");
    var APP_ASSETS_PATH = m_cfg.get_std_assets_path();


    exports.init = function() {
        m_app.init({
            canvas_container_id: "main_canvas_container",
            callback: init_cb,
            show_fps: true,
            autoresize: true,
            assets_dds_available: !DEBUG,
            assets_min50_available: !DEBUG,
            console_verbose: true
        });
    }

    function init_cb(canvas_elem, success) {

        if (!success) {
            console.log("b4w init failure");
            return;
        }
        load();
    }

    function load() {

        var preloader_cont = document.getElementById("preloader_cont");
        m_data.load("./TeeShirtToExport.json", load_cb);
    }
    
    function preloader_cb(percentage) {
        var prelod_dynamic_path = document.getElementById("prelod_dynamic_path");
        var percantage_num = prelod_dynamic_path.nextElementSibling;

        prelod_dynamic_path.style.width = percentage + "%";
        percantage_num.innerHTML = percentage + "%";
        if (percentage == 100) {
            var preloader_cont = document.getElementById("preloader_cont");
            preloader_cont.style.visibility = "hidden";
            return;
        }
    }

    function load_cb(data_id, success) {
        if (!success) {
            console.log("b4w load failure");
            return;
        }
        m_app.enable_camera_controls(false, false, false, null, true);
        //load_data();
    }

    function load_data() {
        var shirt = m_scenes.get_object_by_name("shirt");
        var ctx_image = m_tex.get_canvas_ctx(shirt, "tex_canvas");

        if (ctx_image) {
            var img = new Image();
            img.src = APP_ASSETS_PATH + "earth.jpg";
            img.onload = function () {
                ctx_image.drawImage(img, 0, 0, ctx_image.canvas.width,
                    ctx_image.canvas.height);
                m_tex.update_canvas_ctx(shirt, "tex_canvas");
            }
        }
    }

    function create_interface(obj) {

    }
});

b4w.require("TeeShirt_main").init();