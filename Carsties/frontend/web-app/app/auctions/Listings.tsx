import React from 'react'
import AuctionCard from './AuctionCard';
import { PagedResult } from '@/types';
import { Auction } from './Auction';

async function getDataAsync(): Promise<PagedResult<Auction>> {
    const res = await fetch('http://localhost:6001/search?pageSize=10');

    if(!res.ok) throw new Error('Failed to fetch data');

    return res.json();
}

export default async function Listings() {
    const data = await getDataAsync();

    return (
        // <div>
        //     {JSON.stringify(data)}
        // </div>
      <div className='grid grid-cols-4 gap-6'>
        {data && data.results.map((auction: any) => (
            <AuctionCard auction={auction} key={auction.id} />
        ))}
      </div>
    )
}
