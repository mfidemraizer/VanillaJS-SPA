module.exports = function (grunt) {
    var path = require("path");

    grunt.initConfig({
        pkg: grunt.file.readJSON("package.json"),

        dust: {
            options: {
                name: function (data) {
                    var fileName = data.file.src[0];
                    fileName = fileName.replace("/", "_");
                    fileName = fileName.replace(path.extname(fileName), "");

                    return "app_" + fileName.toLowerCase();
                }
            },
            build: {
                expand: true,
                cwd: "./views",
                src: "**/*.html",
                dest: "./compiledTemplates",
                ext: ".js",
                filter: "isFile"
            }
        },

        concat: {
            options: {
                separator: "\n",
            },
            dist: {
                src: "./compiledTemplates/*.js",
                dest: "./js/ui-templates.js",
            }
        },

        jsbeautifier: {
            files: ["./*.html"]
        },

        less: {
            development: {
                options: {
                    paths: ["./styles"]
                },
                files: {
                    "./styles/styles.css": "./styles/styles.less"
                }
            }
        },

        watch: {
            templates: {
                files: ["./views/**/*.html"],
                tasks: ["dust", "dust-static-templates", "concat", "jsbeautifier"],
                options: {
                    event: ["all"]
                }
            },

            styles: {
                files: ["./styles/**/*.less"],
                tasks: ["less"],
                options: {
                    event: ["all"]
                }
            }
        }
    });

    grunt.registerTask("dust-static-templates", "Compiles static files", function () {
        var fs = require("fs");
        var dust = require("dustjs-linkedin");


        var compiledTemplate = dust.compile(fs.readFileSync("./views/master.html", "utf8"), "app_views_master");
        dust.loadSource(compiledTemplate);

        compiledTemplate = dust.compile(fs.readFileSync("./views/index.html", "utf8"), "app_views_index");
        dust.loadSource(compiledTemplate);

        dust.render("app_views_index", {}, function (err, text) {
            fs.writeFileSync("./index.html", text);
        });
    });

    grunt.loadNpmTasks("grunt-contrib-watch");
    grunt.loadNpmTasks("grunt-contrib-concat");
    grunt.loadNpmTasks("grunt-dustjs-linkedin");
    grunt.loadNpmTasks('grunt-jsbeautifier');
    grunt.loadNpmTasks("grunt-contrib-less");

    grunt.registerTask("default", ["dust", "concat"]);
    grunt.registerTask("develop", ["watch"]);
};