import { getServerSession } from 'next-auth'
import { authOptions } from '../api/auth/[...nextauth]/route'

import React from 'react'
import { NextApiRequest } from 'next';
import { getToken } from 'next-auth/jwt';

import { cookies, headers } from 'next/headers';

export async function getSession() {
    return await getServerSession(authOptions);
}

export async function getCurrentUser() {
    try {
        let session = await getSession();

        if (!session) return null;

        return session.user;
    }
    catch (err) {
        return null;
    }
}

export async function getTokenWorkaround() {
    const req = {
        headers: Object.fromEntries(headers() as Headers),
        cookies: Object.fromEntries(
            cookies()
                .getAll()
                .map(c => [c.name, c.value])
        )
    } as NextApiRequest;

    return await getToken({req});
}