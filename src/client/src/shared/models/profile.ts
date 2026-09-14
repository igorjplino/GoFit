export type Profile = {
    name: string,
    email: string
}

export type UpdateProfileRequest = {
    name: string
}

export type ChangePasswordRequest = {
    currentPassword: string,
    newPassword: string
}
