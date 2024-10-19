describe('Protected Route', () => {
  it('should access protected route with JWT', () => {
    // Assuming you have a valid JWT token
    const jwtToken = 'eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiI4ODkzOWZjYS1jNjZkLTQ4MjUtYTU3Zi0wYTQxMWJmMjJhNjYiLCJlbWFpbCI6ImV4YW1wbGVAZXhhbXBsZS5jb20iLCJnaXZlbl9uYW1lIjoiVXNlcm5hbWVsb2wiLCJuYmYiOjE3MjkzMzIwODUsImV4cCI6MTcyOTkzNjg4NSwiaWF0IjoxNzI5MzMyMDg1LCJpc3MiOiJodHRwOi8vbG9jYWxob3N0OjUwNjMiLCJhdWQiOiJodHRwOi8vbG9jYWxob3N0OjUwNjMifQ.8DyHp6Rl3vWHRzlhlF9iJ6HYJPjvNEsnJZ9Kkbbtro82CQkNxGCwfOGRVZ9JsaSVHO5WpEF79pHC_MRZo87m9w'

    // Use the custom command to set the JWT in localStorage
    cy.loginByJWT(jwtToken);

    // Visit the protected route
    cy.visit('/home');

    // Add assertions to verify successful login
    cy.url().should('include', '/home');
    // Add more assertions as needed

    cy.get('.pointer').first().click();

    cy.url().should('include', '/comments');
  });
});
