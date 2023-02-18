/**
 * @license Copyright (c) 2003-2022, CKSource Holding sp. z o.o. All rights reserved.
 * For licensing, see https://ckeditor.com/legal/ckeditor-oss-license
 */

CKEDITOR.editorConfig = function(config) {
    config.extraPlugins = "N1ED-editor";
    config.skin = "n1theme"; // own CKEditor theme, included into ZIP
    config.removePlugins = "iframe"; // N1ED needs IFrame plugin to be removed, it has own support of iframes
}

CKEDITOR.replace('BlogContent');