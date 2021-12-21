# SecureMicroservices
As a part of the requirements for a real enterprise project, I needed to develop a PoC solution that was dealing with securing microservices using <b>Identity Server 4</b> with <b>Ocelot API Gateway</b>. 
The requirements were to protect our <b>ASP.NET Web MVC</b> and <b>API</b> applications by using <b>OAuth 2</b> and <b>OpenID Connect</b> in <b>IdentityServer4</b>. 
This solution was made out of four main projects which communicate with each other. 
<ul>
	<li>API</li>
  <li>MVC application (Client)</li>
  <li>Identity Server</li>
  <li>Ocelot API gateway</li>
</ul>

Api project uses an in-memory database, with seeded users: bob (password: b1) and alice (password: a1),
so when you start projects, in the MVC client application you can click on the movies tab in the header, and you
will be redirected to Identity Server login, where you can log as bob or alice, bob also has an admin role.
When you log in to Identity Server it will provide you authentication and access control.
Once the client has a bearer token it will call the API endpoint which is fronted by Ocelot. Ocelot is working as a reverse proxy.
After Ocelot reroutes the request to the internal API, it will present the token to the Identity Server in the authorization pipeline.
If the client is authorized the request will be processed and a list of movies will be sent back to the client. 
