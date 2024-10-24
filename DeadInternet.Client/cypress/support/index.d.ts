declare namespace Cypress {
  interface Chainable<Subject = any> {
    loginByJWT(token: string): Chainable<void>
  }
}
