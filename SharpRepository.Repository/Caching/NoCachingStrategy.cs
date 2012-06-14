﻿namespace SharpRepository.Repository.Caching
{
    /// <summary>
    /// Implements no caching within the repository.
    /// </summary>
    /// <typeparam name="T">The type of the repository entity.</typeparam>
    /// <typeparam name="TKey">The type of the primary key.</typeparam>
    public class NoCachingStrategy<T, TKey> : NoCachingStrategyBase<T, TKey>
    {
        
    }

    /// <summary>
    /// Implements no caching within the repository with the primary key of the entity being Int32
    /// </summary>
    /// <typeparam name="T">The type of the repository entity.</typeparam>
    public class NoCachingStrategy<T> : NoCachingStrategyBase<T, int>
    {

    }
}