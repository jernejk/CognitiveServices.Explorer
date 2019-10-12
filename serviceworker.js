// Based on https://medium.com/@k.l.mueller/create-progressive-web-apps-with-net-using-blazor-6aa719e38000
console.log("This is service worker talking!");
var cacheName = 'blazor-pwa-sample';
var filesToCache = [
    './',
    //Html and css files
    './index.html',
    './css/site.css',
    './css/bootstrap/bootstrap.min.css',
    './css/open-iconic/font/css/open-iconic-bootstrap.min.css',
    './css/open-iconic/font/fonts/open-iconic.woff',
    //Blazor framework
    './_framework/blazor.webassembly.js',
    './_framework/blazor.boot.json',
    //Our additional files
    './manifest.json',
    './serviceworker.js',
    './android-chrome-192x192.png',
    './android-chrome-512x512.png',
    './favicon-16x16.png',
    './favicon-32x32.png',
    './apple-touch-icon.png',
    './favicon.ico',
    './site-webmanifest.json',
    //The web assembly/.net dll's
    './_framework/wasm/mono.js',
    './_framework/wasm/mono.wasm',
    './_framework/_bin/Blazored.LocalStorage.dll',
    './_framework/_bin/Microsoft.AspNetCore.Authorization.dll',
    './_framework/_bin/Microsoft.AspNetCore.Blazor.dll',
    './_framework/_bin/Microsoft.AspNetCore.Blazor.HttpClient.dll',
    './_framework/_bin/Microsoft.AspNetCore.Components.dll',
    './_framework/_bin/Microsoft.AspNetCore.Components.Forms.dll',
    './_framework/_bin/Microsoft.AspNetCore.Components.Web.dll',
    './_framework/_bin/Microsoft.AspNetCore.Metadata.dll',
    './_framework/_bin/Microsoft.Bcl.AsyncInterfaces.dll',
    './_framework/_bin/Microsoft.Extensions.Caching.Abstractions.dll',
    './_framework/_bin/Microsoft.Extensions.Caching.Memory.dll',
    './_framework/_bin/Microsoft.Extensions.DependencyInjection.Abstractions.dll',
    './_framework/_bin/Microsoft.Extensions.DependencyInjection.dll',
    './_framework/_bin/Microsoft.Extensions.Logging.Abstractions.dll',
    './_framework/_bin/Microsoft.Extensions.Options.dll',
    './_framework/_bin/Microsoft.Extensions.Primitives.dll',
    './_framework/_bin/Microsoft.JSInterop.dll',
    './_framework/_bin/Mono.Security.dll',
    './_framework/_bin/Mono.WebAssembly.Interop.dll',
    './_framework/_bin/mscorlib.dll',
    './_framework/_bin/Newtonsoft.Json.dll',
    './_framework/_bin/System.Buffers.dll',
    './_framework/_bin/System.ComponentModel.Annotations.dll',
    './_framework/_bin/System.Core.dll',
    './_framework/_bin/System.Data.dll',
    './_framework/_bin/System.dll',
    './_framework/_bin/System.Memory.dll',
    './_framework/_bin/System.Net.Http.dll',
    './_framework/_bin/System.Numerics.dll',
    './_framework/_bin/System.Numerics.Vectors.dll',
    './_framework/_bin/System.Runtime.CompilerServices.Unsafe.dll',
    './_framework/_bin/System.Runtime.Serialization.dll',
    './_framework/_bin/System.Text.Encodings.Web.dll',
    './_framework/_bin/System.Text.Json.dll',
    './_framework/_bin/System.Threading.Tasks.Extensions.dll',
    './_framework/_bin/System.Xml.dll',
    './_framework/_bin/System.Xml.Linq.dll',
    //The compiled project .dll's
    './_framework/_bin/CognitiveServices.Explorer.Domain.dll',
    './_framework/_bin/CognitiveServices.Explorer.Web.dll',
    './_framework/_bin/CognitiveServices.Explorer.Domain.pdb',
    './_framework/_bin/CognitiveServices.Explorer.Web.pdb'
];

self.addEventListener('install', function (e) {
    console.log('[ServiceWorker] Install');
    e.waitUntil(
        caches.open(cacheName).then(function (cache) {
            console.log('[ServiceWorker] Caching app shell');
            return cache.addAll(filesToCache);
        })
    );
});

self.addEventListener('activate', event => {
    event.waitUntil(self.clients.claim());
});

self.addEventListener('fetch', event => {
    event.respondWith(
        caches.match(event.request, { ignoreSearch: true }).then(response => {
            return response || fetch(event.request);
        })
    );
});