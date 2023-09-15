'use server'

import { Auction, PagedResult } from '@/types';

export async function getDataAsync(query: string): Promise<PagedResult<Auction>> {
    const res = await fetch(`http://localhost:6001/search${query}`,
    {
        headers: {
            'Cache-Control': 'no-store'
        }
    });

    if(!res.ok) throw new Error(`Failed to fetch data on ${query}`);

    return res.json();
}