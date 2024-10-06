// Bun v1.1.27+ required
Bun.serve({
  static: {
    // health-check endpoint
    "/api/health-check": new Response("All good!"),

    // redirect from /old-link to /new-link
    //"/old-link": Response.redirect("/new-link", 301),

    // serve static text
    "/": new Response("Hello World"),

    // serve a file by buffering it in memory
    "/index.html": new Response(await Bun.file("index.html").bytes(), {
      headers: {
        "Content-Type": "text/html",
      },
    }),
    "/favicon.png": new Response(await Bun.file("./favicon.png").bytes(), {
      headers: {
        "Content-Type": "image/png",
      },
    }),

    // serve JSON
    "/api/version.json": Response.json({ version: "1.0.0" }),
  },

  fetch(req) {
    return new Response("404!");
  },
});
