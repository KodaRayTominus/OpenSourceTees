"use strict"

import b4w from "blend4web";

var m_app = b4w.app;
var m_data = b4w.data;
var m_scs = b4w.scenes;
var m_cfg = b4w.config;
var m_quat = b4w.quat;
var m_armat = b4w.armature;
var m_tsr = b4w.tsr;
var m_phy = b4w.physics;
var m_trans = b4w.transform;
var m_vec3 = b4w.vec3;
var m_util = b4w.util;
var m_cam = b4w.camera;
var m_version = b4w.version;

var DEBUG = (m_version.type() === "DEBUG");

export function init() {
    m_app.init({
        canvas_container_id: "main_canvas_container",
        callback: init_cb,
        physics_enabled: false,
        alpha: true,
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
    m_data.load(APP_ASSETS_PATH + "IMAGE", load_cb);
}

function load_cb(data_id) {
    m_app.enable_camera_controls(false, false, false, null, true);
    var container = m_cont.get_canvas();
    _world = m_scenes.get_world_by_name("World");
}

function main_canvas_clicked_cb(e) {

    var x = m_mouse.get_coords_x(e);
    var y = m_mouse.get_coords_y(e);

    var obj = m_scenes.pick_object(x, y);
    if (obj) {
    }
}

init();
