import { roles } from '@/_constants'

export const rolesHelper = {
  isTeamRole1GreaterThanRole2
}

function isTeamRole1GreaterThanRole2(role1: string, role2: string) {
  switch (role1) {
    case roles.teamLeader:
      return role2 === roles.teamCoLeader || role2 === roles.teamMember
    case roles.teamCoLeader:
      return role2 === roles.teamMember
    default:
      return false
  }
}
