using EQX.Core.InOut;
using EQX.InOut.Virtual;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public class VirtualIO
    {
        private readonly IDInputDevice _inWorkConveyorInput;
        private readonly IDOutputDevice _inWorkConveyorOutput;
        private readonly IDInputDevice _outWorkConveyorInput;
        private readonly IDOutputDevice _outWorkConveyorOutput;
        private readonly IDInputDevice _robotLoadInput;
        private readonly IDOutputDevice _robotLoadOutput;
        private readonly IDInputDevice _vinylCleanInput;
        private readonly IDOutputDevice _vinylCleanOutput;
        private readonly IDInputDevice _fixtureAlignInput;
        private readonly IDOutputDevice _fixtureAlignOutput;
        private readonly IDInputDevice _removeFilmInput;
        private readonly IDOutputDevice _removeFilmOutput;
        private readonly IDInputDevice _transferFixtureInput;
        private readonly IDOutputDevice _transferFixtureOutput;
        private readonly IDInputDevice _detachInput;
        private readonly IDOutputDevice _detachOutput;
        private readonly IDInputDevice _glassTransferInput;
        private readonly IDOutputDevice _glassTransferOutput;
        private readonly IDInputDevice _glassAlignLeftInput;
        private readonly IDOutputDevice _glassAlignLeftOutput;
        private readonly IDInputDevice _glassAlignRightInput;
        private readonly IDOutputDevice _glassAlignRightOutput;
        private readonly IDInputDevice _transferInShuttleLeftInput;
        private readonly IDOutputDevice _transferInShuttleLeftOutput;
        private readonly IDInputDevice _transferInShuttleRightInput;
        private readonly IDOutputDevice _transferInShuttleRightOutput;

        public VirtualIO([FromKeyedServices("InWorkConveyorInput")] IDInputDevice inWorkConveyorInput,
                         [FromKeyedServices("InWorkConveyorOutput")] IDOutputDevice inWorkConveyorOutput,
                         [FromKeyedServices("OutWorkConveyorInput")] IDInputDevice outWorkConveyorInput,
                         [FromKeyedServices("OutWorkConveyorOutput")] IDOutputDevice outWorkConveyorOutput,
                         [FromKeyedServices("RobotLoadInput")]IDInputDevice robotLoadInput,
                         [FromKeyedServices("RobotLoadOutput")] IDOutputDevice robotLoadOutput,
                         [FromKeyedServices("VinylCleanInput")] IDInputDevice vinylCleanInput,
                         [FromKeyedServices("VinylCleanOutput")] IDOutputDevice vinylCleanOutput,
                         [FromKeyedServices("FixtureAlignInput")] IDInputDevice fixtureAlignInput,
                         [FromKeyedServices("FixtureAlignOutput")] IDOutputDevice fixtureAlignOutput,
                         [FromKeyedServices("RemoveFilmInput")] IDInputDevice removeFilmInput,
                         [FromKeyedServices("RemoveFilmOutput")] IDOutputDevice removeFilmOutput,
                         [FromKeyedServices("TransferFixtureInput")] IDInputDevice transferFixtureInput,
                         [FromKeyedServices("TransferFixtureOutput")] IDOutputDevice transferFixtureOutput,
                         [FromKeyedServices("DetachInput")] IDInputDevice detachInput,
                         [FromKeyedServices("DetachOutput")] IDOutputDevice detachOutput,
                         [FromKeyedServices("GlassTransferInput")] IDInputDevice glassTransferInput,
                         [FromKeyedServices("GlassTransferOutput")] IDOutputDevice glassTransferOutput,
                         [FromKeyedServices("GlassAlignLeftInput")] IDInputDevice glassAlignLeftInput,
                         [FromKeyedServices("GlassAlignLeftOutput")] IDOutputDevice glassAlignLeftOutput,
                         [FromKeyedServices("GlassAlignRightInput")] IDInputDevice glassAlignRightInput,
                         [FromKeyedServices("GlassAlignRightOutput")] IDOutputDevice glassAlignRightOutput,
                         [FromKeyedServices("TransferInShuttleLeftInput")] IDInputDevice transferInShuttleLeftInput,
                         [FromKeyedServices("TransferInShuttleLeftOutput")] IDOutputDevice transferInShuttleLeftOutput,
                         [FromKeyedServices("TransferInShuttleRightInput")] IDInputDevice transferInShuttleRightInput,
                         [FromKeyedServices("TransferInShuttleRightOutput")] IDOutputDevice transferInShuttleRightOutput)
        {
            _inWorkConveyorInput = inWorkConveyorInput;
            _inWorkConveyorOutput = inWorkConveyorOutput;
            _outWorkConveyorInput = outWorkConveyorInput;
            _outWorkConveyorOutput = outWorkConveyorOutput;
            _robotLoadInput = robotLoadInput;
            _robotLoadOutput = robotLoadOutput;
            _vinylCleanInput = vinylCleanInput;
            _vinylCleanOutput = vinylCleanOutput;
            _fixtureAlignInput = fixtureAlignInput;
            _fixtureAlignOutput = fixtureAlignOutput;
            _removeFilmInput = removeFilmInput;
            _removeFilmOutput = removeFilmOutput;
            _transferFixtureInput = transferFixtureInput;
            _transferFixtureOutput = transferFixtureOutput;
            _detachInput = detachInput;
            _detachOutput = detachOutput;
            _glassTransferInput = glassTransferInput;
            _glassTransferOutput = glassTransferOutput;
            _glassAlignLeftInput = glassAlignLeftInput;
            _glassAlignLeftOutput = glassAlignLeftOutput;
            _glassAlignRightInput = glassAlignRightInput;
            _glassAlignRightOutput = glassAlignRightOutput;
            _transferInShuttleLeftInput = transferInShuttleLeftInput;
            _transferInShuttleLeftOutput = transferInShuttleLeftOutput;
            _transferInShuttleRightInput = transferInShuttleRightInput;
            _transferInShuttleRightOutput = transferInShuttleRightOutput;
        }

        public void Initialize()
        {
            _inWorkConveyorInput.Initialize();
            _inWorkConveyorOutput.Initialize();
            _outWorkConveyorInput.Initialize();
            _outWorkConveyorOutput.Initialize();
            _robotLoadInput.Initialize();
            _robotLoadOutput.Initialize();
            _vinylCleanInput.Initialize();
            _vinylCleanOutput.Initialize();
            _fixtureAlignInput.Initialize();
            _fixtureAlignOutput.Initialize();
            _removeFilmInput.Initialize();
            _removeFilmOutput.Initialize();
            _transferFixtureInput.Initialize();
            _transferFixtureOutput.Initialize();
            _detachInput.Initialize();
            _detachOutput.Initialize();
            _glassTransferInput.Initialize();
            _glassTransferOutput.Initialize();
            _glassAlignLeftInput.Initialize();
            _glassAlignLeftOutput.Initialize();
            _glassAlignRightInput.Initialize();
            _glassAlignRightOutput.Initialize();
            _transferInShuttleLeftInput.Initialize();
            _transferInShuttleRightInput.Initialize();
            _transferInShuttleLeftOutput.Initialize();
            _transferInShuttleRightOutput.Initialize();
        }

        public void Mappings()
        {
            //InWorkConveyor Input Mapping
            ((VirtualInputDevice<EInWorkConveyorProcessInput>)_inWorkConveyorInput).Mapping((int)EInWorkConveyorProcessInput.ROBOT_PICK_IN_CST_DONE,
                _robotLoadOutput, (int)ERobotLoadProcessOutput.ROBOT_PICK_IN_CST_DONE);

            //OutWorkConveyor Input Mapping
            ((VirtualInputDevice<EOutWorkConveyorProcessInput>)_outWorkConveyorInput).Mapping((int)EOutWorkConveyorProcessInput.ROBOT_PLACE_OUT_CST_DONE,
                _robotLoadOutput, (int)ERobotLoadProcessOutput.ROBOT_PLACE_OUT_CST_DONE);

            //Robot Load Input Mapping
            ((VirtualInputDevice<ERobotLoadProcessInput>)_robotLoadInput).Mapping((int)ERobotLoadProcessInput.IN_CST_READY,
                _inWorkConveyorOutput, (int)EInWorkConveyorProcessOutput.IN_CST_READY);
            ((VirtualInputDevice<ERobotLoadProcessInput>)_robotLoadInput).Mapping((int)ERobotLoadProcessInput.OUT_CST_READY,
                _outWorkConveyorOutput, (int)EOutWorkConveyorProcessOutput.OUT_CST_READY);
            ((VirtualInputDevice<ERobotLoadProcessInput>)_robotLoadInput).Mapping((int)ERobotLoadProcessInput.VINYL_CLEAN_REQ_LOAD,
                _vinylCleanOutput, (int)EVinylCleanProcessOutput.VINYL_CLEAN_REQ_LOAD);
            ((VirtualInputDevice<ERobotLoadProcessInput>)_robotLoadInput).Mapping((int)ERobotLoadProcessInput.VINYL_CLEAN_RECEIVE_LOAD_DONE,
                _vinylCleanOutput, (int)EVinylCleanProcessOutput.VINYL_CLEAN_RECEIVE_LOAD_DONE);
            ((VirtualInputDevice<ERobotLoadProcessInput>)_robotLoadInput).Mapping((int)ERobotLoadProcessInput.VINYL_CLEAN_REQ_UNLOAD,
                _vinylCleanOutput, (int)EVinylCleanProcessOutput.VINYL_CLEAN_REQ_UNLOAD);
            ((VirtualInputDevice<ERobotLoadProcessInput>)_robotLoadInput).Mapping((int)ERobotLoadProcessInput.VINYL_CLEAN_RECEIVE_UNLOAD_DONE,
                _vinylCleanOutput, (int)EVinylCleanProcessOutput.VINYL_CLEAN_RECEIVE_UNLOAD_DONE);
            ((VirtualInputDevice<ERobotLoadProcessInput>)_robotLoadInput).Mapping((int)ERobotLoadProcessInput.FIXTURE_ALIGN_REQ_LOAD,
                _fixtureAlignOutput, (int)EFixtureAlignProcessOutput.FIXTURE_ALIGN_REQ_LOAD);
            ((VirtualInputDevice<ERobotLoadProcessInput>)_robotLoadInput).Mapping((int)ERobotLoadProcessInput.REMOVE_FILM_REQ_UNLOAD,
                _removeFilmOutput, (int)ERemoveFilmProcessOutput.REMOVE_FILM_REQ_UNLOAD);
            ((VirtualInputDevice<ERobotLoadProcessInput>)_robotLoadInput).Mapping((int)ERobotLoadProcessInput.IN_CST_PICK_DONE,
                _inWorkConveyorOutput, (int)EInWorkConveyorProcessOutput.IN_CST_PICK_DONE);
            ((VirtualInputDevice<ERobotLoadProcessInput>)_robotLoadInput).Mapping((int)ERobotLoadProcessInput.OUT_CST_PLACE_DONE,
                _outWorkConveyorOutput, (int)EOutWorkConveyorProcessOutput.OUT_CST_PLACE_DONE);

            //Vinyl Clean Input Mapping
            ((VirtualInputDevice<EVinylCleanProcessInput>)_vinylCleanInput).Mapping((int)EVinylCleanProcessInput.VINYL_CLEAN_LOAD_DONE,
                _robotLoadOutput, (int)ERobotLoadProcessOutput.VINYL_CLEAN_LOAD_DONE);
            ((VirtualInputDevice<EVinylCleanProcessInput>)_vinylCleanInput).Mapping((int)EVinylCleanProcessInput.VINYL_CLEAN_UNLOAD_DONE,
                _robotLoadOutput, (int)ERobotLoadProcessOutput.VINYL_CLEAN_UNLOAD_DONE);

            //Fixture Align Input Mapping
            ((VirtualInputDevice<EFixtureAlignProcessInput>)_fixtureAlignInput).Mapping((int)EFixtureAlignProcessInput.FIXTURE_ALIGN_LOAD_DONE,
                _robotLoadOutput, (int)ERobotLoadProcessOutput.FIXTURE_ALIGN_LOAD_DONE);
            ((VirtualInputDevice<EFixtureAlignProcessInput>)_fixtureAlignInput).Mapping((int)EFixtureAlignProcessInput.FIXTURE_TRANSFER_DONE,
                _transferFixtureOutput, (int)ETransferFixtureProcessOutput.FIXTURE_TRANSFER_DONE);

            //Remove Film Input Mapping
            ((VirtualInputDevice<ERemoveFilmProcessInput>)_removeFilmInput).Mapping((int)ERemoveFilmProcessInput.REMOVE_FILM_UNLOAD_DONE,
                _robotLoadOutput, (int)ERobotLoadProcessOutput.REMOVE_FILM_UNLOAD_DONE);
            ((VirtualInputDevice<ERemoveFilmProcessInput>)_removeFilmInput).Mapping((int)ERemoveFilmProcessInput.FIXTURE_TRANSFER_DONE,
                _transferFixtureOutput, (int)ETransferFixtureProcessOutput.FIXTURE_TRANSFER_DONE);

            //Transfer Fixture Input Mapping
            ((VirtualInputDevice<ETransferFixtureProcessInput>)_transferFixtureInput).Mapping((int)ETransferFixtureProcessInput.DETACH_ORIGIN_DONE,
                _detachOutput, (int)EDetachProcessOutput.DETACH_ORIGIN_DONE);
            ((VirtualInputDevice<ETransferFixtureProcessInput>)_transferFixtureInput).Mapping((int)ETransferFixtureProcessInput.DETACH_DONE,
                _detachOutput, (int)EDetachProcessOutput.DETACH_DONE);
            ((VirtualInputDevice<ETransferFixtureProcessInput>)_transferFixtureInput).Mapping((int)ETransferFixtureProcessInput.FIXTURE_ALIGN_DONE,
                _fixtureAlignOutput, (int)EFixtureAlignProcessOutput.FIXTURE_ALIGN_DONE);
            ((VirtualInputDevice<ETransferFixtureProcessInput>)_transferFixtureInput).Mapping((int)ETransferFixtureProcessInput.REMOVE_FILM_DONE,
                _removeFilmOutput, (int)ERemoveFilmProcessOutput.REMOVE_FILM_DONE);
            ((VirtualInputDevice<ETransferFixtureProcessInput>)_transferFixtureInput).Mapping((int)ETransferFixtureProcessInput.ALIGN_TRANSFER_FIXTURE_DONE_RECEIVED,
                _fixtureAlignOutput, (int)EFixtureAlignProcessOutput.TRANSFER_FIXTURE_DONE_RECEIVED);
            ((VirtualInputDevice<ETransferFixtureProcessInput>)_transferFixtureInput).Mapping((int)ETransferFixtureProcessInput.DETACH_TRANSFER_FIXTURE_DONE_RECEIVED,
                _detachOutput, (int)EDetachProcessOutput.TRANSFER_FIXTURE_DONE_RECEIVED);
            ((VirtualInputDevice<ETransferFixtureProcessInput>)_transferFixtureInput).Mapping((int)ETransferFixtureProcessInput.REMOVE_FILM_TRANSFER_FIXTURE_DONE_RECEIVED,
                _removeFilmOutput, (int)ERemoveFilmProcessOutput.TRANSFER_FIXTURE_DONE_RECEIVED);

            //Detach Input Mapping
            ((VirtualInputDevice<EDetachProcessInput>)_detachInput).Mapping((int)EDetachProcessInput.FIXTURE_TRANSFER_DONE,
                _transferFixtureOutput, (int)ETransferFixtureProcessOutput.FIXTURE_TRANSFER_DONE);
            ((VirtualInputDevice<EDetachProcessInput>)_detachInput).Mapping((int)EDetachProcessInput.GLASS_TRANSFER_PICK_DONE,
                _glassTransferOutput, (int)EGlassTransferProcessOutput.GLASS_TRANSFER_PICK_DONE);

            //Glass Transfer Input Mapping
            ((VirtualInputDevice<EGlassTransferProcessInput>)_glassTransferInput).Mapping((int)EGlassTransferProcessInput.DETACH_REQ_UNLOAD_GLASS,
                _detachOutput, (int)EDetachProcessOutput.DETACH_REQ_UNLOAD_GLASS);
            ((VirtualInputDevice<EGlassTransferProcessInput>)_glassTransferInput).Mapping((int)EGlassTransferProcessInput.GLASS_TRANSFER_PICK_DONE_RECEIVED,
                _detachOutput, (int)EDetachProcessOutput.GLASS_TRANSFER_PICK_DONE_RECEIVED);
            ((VirtualInputDevice<EGlassTransferProcessInput>)_glassTransferInput).Mapping((int)EGlassTransferProcessInput.GLASS_ALIGN_LEFT_REQ_GLASS,
                _glassAlignLeftOutput, (int)EGlassAlignProcessOutput.GLASS_ALIGN_REQ_GLASS);
            ((VirtualInputDevice<EGlassTransferProcessInput>)_glassTransferInput).Mapping((int)EGlassTransferProcessInput.GLASS_ALIGN_RIGHT_REQ_GLASS,
                _glassAlignRightOutput, (int)EGlassAlignProcessOutput.GLASS_ALIGN_REQ_GLASS);
            ((VirtualInputDevice<EGlassTransferProcessInput>)_glassTransferInput).Mapping((int)EGlassTransferProcessInput.GLASS_ALIGN_LEFT_PLACE_DONE_RECEIVED,
                _glassAlignLeftOutput, (int)EGlassAlignProcessOutput.GLASS_ALIGN_PLACE_DONE_RECEIVED);
            ((VirtualInputDevice<EGlassTransferProcessInput>)_glassTransferInput).Mapping((int)EGlassTransferProcessInput.GLASS_ALIGN_RIGHT_PLACE_DONE_RECEIVED,
                _glassAlignRightOutput, (int)EGlassAlignProcessOutput.GLASS_ALIGN_PLACE_DONE_RECEIVED);

            //Glass Align Input Mapping
            //Left
            ((VirtualInputDevice<EGlassAlignProcessInput>)_glassAlignLeftInput).Mapping((int)EGlassAlignProcessInput.GLASS_TRANSFER_PLACE_DONE,
                _glassTransferOutput, (int)EGlassTransferProcessOutput.GLASS_TRANSFER_LEFT_PLACE_DONE);
            //Right
            ((VirtualInputDevice<EGlassAlignProcessInput>)_glassAlignRightInput).Mapping((int)EGlassAlignProcessInput.GLASS_TRANSFER_PLACE_DONE,
                _glassTransferOutput, (int)EGlassTransferProcessOutput.GLASS_TRANSFER_RIGHT_PLACE_DONE);

            //TransferInShuttle Input Mapping
            //Left
            ((VirtualInputDevice<ETransferInShuttleProcessInput>)_transferInShuttleLeftInput).Mapping((int)ETransferInShuttleProcessInput.GLASS_ALIGN_REQ_PICK,
                _glassAlignLeftOutput, (int)EGlassAlignProcessOutput.GLASS_ALIGN_REQ_PICK);
            ((VirtualInputDevice<ETransferInShuttleProcessInput>)_transferInShuttleLeftInput).Mapping((int)ETransferInShuttleProcessInput.GLASS_ALIGN_PICK_DONE_RECEIVED,
                _glassAlignLeftOutput, (int)EGlassAlignProcessOutput.GLASS_ALIGN_PICK_DONE_RECEIVED);
            //Right
            ((VirtualInputDevice<ETransferInShuttleProcessInput>)_transferInShuttleRightInput).Mapping((int)ETransferInShuttleProcessInput.GLASS_ALIGN_REQ_PICK,
                _glassAlignRightOutput, (int)EGlassAlignProcessOutput.GLASS_ALIGN_REQ_PICK);
            ((VirtualInputDevice<ETransferInShuttleProcessInput>)_transferInShuttleRightInput).Mapping((int)ETransferInShuttleProcessInput.GLASS_ALIGN_PICK_DONE_RECEIVED,
                _glassAlignRightOutput, (int)EGlassAlignProcessOutput.GLASS_ALIGN_PICK_DONE_RECEIVED);
        }
    }
}
