using System.Linq;

namespace Aoc2019_Day14
{
    internal class ChemicalReactor
    {
        private readonly ChemicalReaction _reaction;
        private          long             _quantityOnHand;
        
        public string ProductName           { get; }
        public long   TotalQuantityProduced { get; private set; }

        public ChemicalReactor(ChemicalReaction reaction)
        {
            _reaction = reaction;
            ProductName = _reaction.Product.Name;
        }

        public void Produce(ChemicalReactor[] reactors, long quantityNeeded)
        {
            if (quantityNeeded < _quantityOnHand)
            {
                _quantityOnHand -= quantityNeeded;
                return;
            }

            quantityNeeded -= _quantityOnHand;
            _quantityOnHand = 0;

            var reactionQuantity        = _reaction.Product.Quantity;
            var numberOfReactionsNeeded = (quantityNeeded + reactionQuantity - 1) / reactionQuantity;
                
            foreach (var reagent in _reaction.Reagents)
            {
                reactors.Single(r => r.ProductName == reagent.Name)
                        .Produce(reactors, reagent.Quantity * numberOfReactionsNeeded);
            }

            var quantityProduced = reactionQuantity * numberOfReactionsNeeded;
            TotalQuantityProduced += quantityProduced;
            _quantityOnHand += quantityProduced - quantityNeeded;
        }
    }
}